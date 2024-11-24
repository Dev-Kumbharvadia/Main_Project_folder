namespace AppAPI.Models.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!; // Ensured non-nullable Username
        public string Email { get; set; } = null!; // Ensured non-nullable Email
        public string PasswordHash { get; set; } = null!; // Ensured non-nullable PasswordHash

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<TransactionHistory> Transactions { get; set; } = new List<TransactionHistory>();
        public ICollection<UserAudit> UserAudits { get; set; } = new List<UserAudit>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
