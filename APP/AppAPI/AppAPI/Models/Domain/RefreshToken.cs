using System;

namespace AppAPI.Models.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; } // Make Revoked nullable
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired; // Check for null
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
