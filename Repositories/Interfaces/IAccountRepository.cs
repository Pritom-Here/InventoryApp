using InventoryApp.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<ApplicationUser> GetUsers();
        Task<ApplicationUser> FindByIdAsync(string id);
        Task<ApplicationUser> FindByNameAsync(string name);
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        IdentityRole GetRoleByName(string roleName);
        Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string role);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    }
}
