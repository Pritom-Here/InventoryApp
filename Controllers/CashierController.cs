using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class CashierController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public CashierController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _accountRepository.GetUsersByRoleAsync(RolesAndPolicies.Roles.Cashier);
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email
                };

                var resultCreateUser = await _accountRepository.CreateUserAsync(user, model.Password);

                if (!resultCreateUser.Succeeded)
                {
                    foreach (var error in resultCreateUser.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View();
                }

                var resultRoleAssign = await _accountRepository.AssignRoleToUserAsync(user, RolesAndPolicies.Roles.Cashier);

                return RedirectToAction("Index", "Cashier");
            }

            return View(model);
        }
    }
}
