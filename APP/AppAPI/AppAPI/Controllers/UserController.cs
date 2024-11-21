using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAPI.Data;
using AppAPI.Models;
using AppAPI.Models.Domain;
using AppAPI.Models.DTO;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("AssignRole")] //ok
        public async Task<ActionResult> UpdateUserRoles(Guid userId, List<Guid> roleIds)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} does not exist.");
            }

            // Retrieve the user's existing roles
            var existingUserRoles = _context.UserRoles.Where(ur => ur.UserId == userId).ToList();

            // Add new roles that the user doesn't already have
            foreach (var roleId in roleIds)
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null)
                {
                    return BadRequest($"Role with ID {roleId} does not exist.");
                }

                var userRoleExist = existingUserRoles.SingleOrDefault(ur => ur.RoleId == roleId);
                if (userRoleExist == null)
                {
                    _context.UserRoles.Add(new UserRole { RoleId = roleId, UserId = userId });
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User roles updated successfully." });
        }

        [HttpPost("RewriteRoles")] //ok
        public async Task<ActionResult> RewriteRoles(Guid userId, List<Guid> roleIds)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} does not exist.");
            }

            // Retrieve the user's existing roles
            var existingUserRoles = _context.UserRoles.Where(ur => ur.UserId == userId).ToList();

            // Remove roles that are not in the provided roleIds list
            foreach (var userRole in existingUserRoles)
            {
                if (!roleIds.Contains(userRole.RoleId))
                {
                    _context.UserRoles.Remove(userRole);
                }
            }

            // Add new roles that the user doesn't already have
            foreach (var roleId in roleIds)
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null)
                {
                    return BadRequest($"Role with ID {roleId} does not exist.");
                }

                var userRoleExist = existingUserRoles.SingleOrDefault(ur => ur.RoleId == roleId);
                if (userRoleExist == null)
                {
                    _context.UserRoles.Add(new UserRole { RoleId = roleId, UserId = userId });
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User roles updated successfully." });
        }

        [HttpPost("AddUser")] //ok
        public ActionResult<ApiResponse<User>> AddUser([FromBody] UserRegisterModel model)
        {
            // Check if the username already exists
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Username already exists",
                    Success = false,
                });
            }

            // Check if the email already exists
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Email already exists",
                    Success = false,
                });
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The registration model cannot be null.");
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                throw new ArgumentException("Username is required.", nameof(model.Username));
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentException("Password is required.", nameof(model.Password));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentException("Email is required.", nameof(model.Email));
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email
            };

            _context.Users.Add(user);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<User>
                {
                    Message = $"Error registering user: {ex.Message}",
                    Success = false,
                });
            }

            return CreatedAtAction(nameof(AddUser), new { id = user.UserId }, new ApiResponse<User>
            {
                Message = "User registered successfully",
                Success = true,
                Data = user
            });
        }

        [HttpGet("GetAllUsers")] //ok
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("GetUserByID")] //ok
        public async Task<ActionResult<IEnumerable<User>>> GetUserByID(Guid Id)
        {
            var users = await _context.Users.FindAsync(Id);
            return Ok(users);
        }

        [HttpPut("UpdateUser")] //ok
        public async Task<ActionResult<IEnumerable<User>>> UpdateUser(Guid Id, UpdateUserDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The update model cannot be null.");
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Username is required.",
                    Success = false,
                });
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Password is required.",
                    Success = false,
                });
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Email is required.",
                    Success = false,
                });
            }

            var user = await _context.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound(new ApiResponse<User>
                {
                    Message = "User not found.",
                    Success = false,
                });
            }

            if (_context.Users.Any(u => u.Username == model.Username && u.UserId != Id))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Username already exists.",
                    Success = false,
                });
            }

            user.Username = model.Username;
            user.Email = model.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<User>
                {
                    Data = user,
                    Message = "User updated successfully.",
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<User>
                {
                    Message = $"An error occurred while updating the user: {ex.Message}",
                    Success = false,
                });
            }
        }

        [HttpDelete("DeleteUser")] //ok
        public async Task<ActionResult<IEnumerable<User>>> DeleteUser(Guid Id)
        {
            var users = await _context.Users.FindAsync(Id);
            if(users == null)
            {
                return BadRequest("User not fond");
            }
            _context.Users.Remove(users);
            _context.SaveChanges();
            return Ok(users);
        }

    }
}
