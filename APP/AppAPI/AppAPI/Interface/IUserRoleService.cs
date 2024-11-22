namespace AppAPI.Models.Interface
{
    public interface IUserRoleService
    {
        Task<bool> UpdateUserRolesAsync(Guid userId, List<Guid> roleIds);
    }

}
