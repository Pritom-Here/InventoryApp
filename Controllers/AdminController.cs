using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    [Authorize(Roles = RolesAndPolicies.Roles.Administrator)]
    public class AdminController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMailer _mailer;
        private readonly ITemplateHelper _templateHelper;

        public AdminController(IAccountRepository accountRepository, IMailer mailer, ITemplateHelper templateHelper)
        {
            _accountRepository = accountRepository;
            _mailer = mailer;
            _templateHelper = templateHelper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _accountRepository.GetUsersByRoleAsync(RolesAndPolicies.Roles.Administrator);
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                 
                var resultRoleAssign = await _accountRepository.AssignRoleToUserAsync(user, RolesAndPolicies.Roles.Administrator);

                var token = await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                var userCredentialsModel = new UserCredentialsViewModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = RolesAndPolicies.Roles.Administrator,
                    Token = token,
                    ConfirmationLink = confirmationLink
                };
                
                //Sending Account Confirmation Email
                //step 1: converting html template to string
                var emailContent = await _templateHelper.GetHtmlTemplateAsStringAsync<UserCredentialsViewModel>("Template/NewSubscriberGreetingTemplate", userCredentialsModel);
                //step 2: sending the mail
                await _mailer.SendEmailAsync(model.Email, $"[Inventory App] Please Confirm Your Account !", emailContent);

                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }
    }
}
