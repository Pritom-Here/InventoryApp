using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager )
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _applicationDbContext.Users.ToList();
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> FindByNameAsync(string name)
        {   
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            return await _signInManager.UserManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            var roleInDb = _roleManager.Roles.FirstOrDefault(r => r.Name == roleName);
            return roleInDb;
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }
    }
}
