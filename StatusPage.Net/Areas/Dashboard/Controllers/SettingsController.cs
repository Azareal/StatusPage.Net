using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StatusPage.Net.Models;
using StatusPage.Net.Models.AccountViewModels;

namespace StatusPage.Net.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public SettingsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<SettingsController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Config()
        {
            return View();
        }

        public async Task<IActionResult> Accounts()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> Roles()
        {
            var users = await _roleManager.Roles.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("name", "Role name cannot be empty.");
                return View();
            }
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("name", "Could not create role.");
                return View();
            }
            return RedirectToAction("Roles");
        }

        [HttpGet]
        public async Task<IActionResult> AddToRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Account with id {id} not found");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string id, string roleId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Account with id {id} not found");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ModelState.AddModelError("roleId", $"Role with id {roleId} could not be found");
                return View(user);
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");

                    // Skip email validation by generating the confirmation token and then using it
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, code);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction("Accounts");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}