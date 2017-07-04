using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusPage.Net.Data;
using StatusPage.Net.Models;
using StatusPage.Net.Models.HomeViewModels;

namespace StatusPage.Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!_db.Settings.Any(x=>x.Id == "Setup"))
            {
                return View("Setup");
            }
            var monthAgo = DateTime.UtcNow.AddDays(-30);
            var dayAgo = DateTime.UtcNow.AddDays(-1);
            var statusMessages = await _db.StatusMessages.Include(x => x.Incident).ThenInclude(x => x.Site).Where(x => x.DateTime > monthAgo).OrderByDescending(x=>x.IncidentId).ThenByDescending(x=>x.DateTime).ToListAsync();
            var pings = (await _db.Pings.Where(x => x.PingSetting.Visible && x.DateTime > dayAgo).ToListAsync()).GroupBy(x=>x.PingSetting);
            var viewModel = new StatusPageViewModel()
            {
                StatusMessages = statusMessages,
                Pings = pings
            };
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> Setup(SetupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _db.Settings.Add(new Setting("Setup", "1"));
            _db.Settings.Add(new Setting("SiteName", model.SiteName));

            await _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true,
            }, model.Password);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
