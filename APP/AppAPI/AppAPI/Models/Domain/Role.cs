namespace AppAPI.Models.Domain
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
