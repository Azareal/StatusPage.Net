using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusPage.Net.Data;
using StatusPage.Net.Models.HomeViewModels;

namespace StatusPage.Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
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
    }
}
