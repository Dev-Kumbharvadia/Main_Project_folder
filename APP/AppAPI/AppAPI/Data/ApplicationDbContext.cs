using AppAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for each entity type
        public DbSet<Product> Products { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAudit> UserAudits { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<DeletedProducts> DeletedProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User to Product Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to RefreshToken Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to UserAudit Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserAudits)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to TransactionHistory Relationship (as Buyer)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne()
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            // TransactionHistory to Product Relationship
            modelBuilder.Entity<TransactionHistory>()
                .HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 

            // TransactionHistory to Buyer (User) Relationship
            modelBuilder.Entity<TransactionHistory>()
                .HasOne(t => t.Buyer)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.Cascade); // Delete transactions if the buyer is deleted

            // TransactionHistory to Seller (User) Relationship
            modelBuilder.Entity<TransactionHistory>()
                .HasOne(t => t.Seller)
                .WithMany() // No need to track Seller's transaction history separately in this case
                .HasForeignKey(t => t.SellerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletion

            // Composite Key for UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // UserRole to User Relationship
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserRole to Role Relationship
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Default Values for Product
            modelBuilder.Entity<Product>()
                .Property(p => p.StockQuantity)
                .HasDefaultValue(0);

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Product>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
