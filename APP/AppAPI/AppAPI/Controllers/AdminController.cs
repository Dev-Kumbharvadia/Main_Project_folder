using AppAPI.Data;
using AppAPI.Models.Domain;
using AppAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AdminController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<User>> DeleteUser(Guid id) // Confirm type matches UserId's type
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // GET: api/audits
        [HttpGet("GetAllAudits")] //ok
        public IActionResult GetUserAudits()
        {
            try
            {
                var audits = _context.UserAudits
                    .Include(a => a.User)
                    .Select(a => new AuditWithUserDTO
                    {
                        UserAuditId = a.UserAuditId,
                        UserId = a.UserId,
                        Username = a.User!.Username,
                        LoginTime = a.LoginTime,
                        LogoutTime = a.LogoutTime
                    })
                    .ToList();

                if (!audits.Any())
                {
                    return NotFound("No audits found.");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/audits/{userId}
        [HttpGet("GetAuditsByUserID")] //ok
        public async Task<IActionResult> GetUserAuditsByUserId(Guid userId)
        {
            try
            {
                var audits = await _context.UserAudits
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                if (!audits.Any())
                {
                    return NotFound($"No audits found for user with ID: {userId}");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
