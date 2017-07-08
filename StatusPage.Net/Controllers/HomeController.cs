using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusPage.Net.Data;
using StatusPage.Net.Misc.Extensions;
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
            var twoMonthsAgo = DateTime.UtcNow.AddDays(-60);
            var dayAgo = DateTime.UtcNow.AddDays(-1);
            var statusMessages = await _db.Incidents.Include(x => x.Messages).Include(x => x.Site).Where(x => x.Start > monthAgo).OrderByDescending(x=>x.Start).ToListAsync();
            var pings = (await _db.Pings.Where(x => x.PingSetting.Visible && x.DateTime > dayAgo).ToListAsync()).GroupBy(x=>x.PingSetting);
            var today = DateTime.Today;
            var dates = Enumerable.Range(1, 60).Select(x => today.AddDays(-x)).ToArray();
            // TODO: Rewrite to include all incidents every day
            var sql = $@"
SELECT CAST([Start] AS DATE) Date, DATEDIFF(SECOND, MIN([End]), MIN([Start])) Duration, COUNT(*) [Count] FROM Incidents
WHERE CAST([Start] AS DATE) > N'{twoMonthsAgo:O}'
GROUP BY CAST([Start] AS DATE)";
            var incidentsPerDay = _db.Database.SqlQuery<IncidentDailySummary>(sql).ToList();
            var pingSummaries = _db.Database.SqlQuery<PingSummary>($@"SELECT SUM(IIF(ResponseTime > 300, 1, 0)) [HighPingCount], COUNT(*) [Count], CAST([DateTime] AS DATE) [Date] FROM Pings WHERE CAST([DateTime] AS DATE) > N'{twoMonthsAgo:O}' GROUP BY CAST([DateTime] AS DATE);").ToList();
            var summaries = dates.Select(x => new DailyStatusSummary()
            {
                Date = x,
                DownTimePercentage = incidentsPerDay.FirstOrDefault(y => y.Date == x)?.Duration ?? 0 / 86400,
                HighPingPercentage = (pingSummaries.FirstOrDefault(y => y.Date == x)?.HighPingCount / pingSummaries.FirstOrDefault(y => y.Date == x)?.Count) ?? 0
            }).OrderBy(x=>x.Date).ToList();
            var viewModel = new StatusPageViewModel()
            {
                Incidents = statusMessages,
                Pings = pings,
                DailyStatusSummaries = summaries
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
