using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<ApplicationUser> GetUsers();
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        IdentityRole GetRoleByName(string roleName);
        Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string role);
    }
}
