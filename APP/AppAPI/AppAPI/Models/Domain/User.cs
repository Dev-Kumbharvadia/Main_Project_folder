namespace AppAPI.Models.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<TransactionHistory> Transactions { get; set; }
        public ICollection<UserAudit> UserAudits { get; set; } = new List<UserAudit>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
