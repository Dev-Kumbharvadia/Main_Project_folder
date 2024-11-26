using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AppAPI.Data;
using AppAPI.Models;
using AppAPI.Models.Domain;
using TodoAPI.Models;
using AppAPI.Models.Interface;
using AppAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<User>>> Register([FromBody] UserRegisterModel model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Username already exists",
                    Success = false,
                });
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Email already exists",
                    Success = false,
                });
            }

            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentException("All fields are required.");
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.UserId }, new ApiResponse<User>
            {
                Message = "User registered successfully",
                Success = true,
                Data = user
            });
        }

        // POST: api/audit/logout/{userId}
        [HttpPost("Logout")] //ok
        [Authorize]
        public IActionResult Logout(Guid userId)
        {
            try
            {
                // Find the last audit entry for the user (the most recent login without a logout time)
                var lastAudit = _context.UserAudits
                    .Where(ua => ua.UserId == userId && ua.LogoutTime == null)
                    .OrderByDescending(ua => ua.LoginTime)
                    .FirstOrDefault();

                if (lastAudit == null)
                {
                    return BadRequest(new ApiResponse<UserAudit>
                    {
                        Message = "No active login session found for this user",
                        Success = false,
                    });
                }

                // Set the logout time to the current time
                lastAudit.LogoutTime = DateTime.UtcNow;
                _context.UserAudits.Update(lastAudit); // Update the audit record in the database
                _context.SaveChanges();

                return Ok(new ApiResponse<UserAudit>
                {
                    Message = "Logout recorded successfully",
                    Success = true,
                    Data = lastAudit
                });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, new ApiResponse<UserAudit>
                {
                    Message = $"An error occurred while processing your request: {ex.Message}",
                    Success = false,
                });
            }
        }

        [HttpPost("Login")]
        public ActionResult<ApiResponse<object>> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Message = "Invalid input data",
                    Success = false,
                });
            }

            var user = _context.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Message = "Invalid credentials",
                    Success = false,
                });
            }

            var userRoles = _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.Role!.RoleName)
                .ToList();

            if (!userRoles.Any())
            {
                return BadRequest(new ApiResponse<object>
                {
                    Message = "User does not have any roles assigned.",
                    Success = false,
                });
            }

            Logout(user.UserId);

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            foreach (var existingToken in user.RefreshTokens)
            {
                existingToken.Revoked = DateTime.UtcNow;
            }

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = user.UserId
            });

            _context.UserAudits.Add(new UserAudit { LoginTime = DateTime.UtcNow, UserId = user.UserId });

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new ApiResponse<object>
                {
                    Message = $"Error logging in: {ex.Message}",
                    Success = false,
                });
            }

            return Ok(new ApiResponse<object>
            {
                Message = "Login successful",
                Success = true,
                Data = new
                {
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken,
                    UserId = user.UserId
                }
            });
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public ActionResult<ApiResponse<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new ApiResponse<RefreshTokenResponse>
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            var refreshToken = request.RefreshToken;
            User user;

            try
            {
                user = _context.Users.Include(u => u.RefreshTokens)
                                     .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RefreshTokenResponse>
                {
                    Success = false,
                    Message = $"Database error: {ex.Message}"
                });
            }

            if (user == null)
            {
                return Unauthorized(new ApiResponse<RefreshTokenResponse>
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            var storedToken = user.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);

            if (storedToken == null)
            {
                return Unauthorized(new ApiResponse<RefreshTokenResponse>
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            if (!storedToken.IsActive)
            {
                return Unauthorized(new ApiResponse<RefreshTokenResponse>
                {
                    Success = false,
                    Message = storedToken.IsExpired ? "Token expired" : "Token has been revoked"
                });
            }

            var newJwtToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                storedToken.Revoked = DateTime.UtcNow;
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = newRefreshToken,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    UserId = user.UserId
                });

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new ApiResponse<RefreshTokenResponse>
                {
                    Success = false,
                    Message = $"Error refreshing token: {ex.Message}"
                });
            }

            return Ok(new ApiResponse<RefreshTokenResponse>
            {
                Success = true,
                Message = "Tokens refreshed successfully",
                Data = new RefreshTokenResponse
                {
                    JwtToken = newJwtToken,
                    RefreshToken = newRefreshToken
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key not configured.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRoles = _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.Role!.RoleName)
                .ToList();

            if (!userRoles.Any())
            {
                throw new InvalidOperationException("User does not have any roles assigned.");
            }

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.Username)
    };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            string issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer not configured.");
            string audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience not configured.");

            double expiresInMinutes = 60;
            if (!string.IsNullOrEmpty(_config["Jwt:ExpiresInMinutes"]) && double.TryParse(_config["Jwt:ExpiresInMinutes"], out var parsedExpiresInMinutes))
            {
                expiresInMinutes = parsedExpiresInMinutes;
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

    }
}
