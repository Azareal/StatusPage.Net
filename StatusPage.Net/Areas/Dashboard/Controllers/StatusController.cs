using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusPage.Net.Data;
using StatusPage.Net.Models.StatusViewModels;

namespace StatusPage.Net.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StatusController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var incidents = await _db.Incidents.Include(x=>x.Messages).Include(x=>x.Site).Where(x => x.End == null).ToListAsync();
            return View(incidents);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateIncidentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateIncidentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var incident = new Incident()
            {
                Name = model.Title,
                Start = model.DateTime ?? DateTime.UtcNow,
            };

            var initialMessage = new StatusMessage()
            {
                Incident = incident,
                Description = model.InitialMessage,
                Status = model.InitialMessageType,
                DateTime = model.DateTime ?? DateTime.UtcNow
            };
            _db.Incidents.Add(incident);
            _db.StatusMessages.Add(initialMessage);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(new CreateStatusMessageViewModel()
            {
                IncidentId = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(CreateStatusMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var incident = await _db.Incidents.FindAsync(model.IncidentId);
            if (incident == null)
            {
                throw new NullReferenceException("Incident with the given ID could not be found");
            }

            var initialMessage = new StatusMessage()
            {
                IncidentId = model.IncidentId,
                Description = model.Message,
                Status = model.Status,
                DateTime = model.DateTime ?? DateTime.UtcNow
            };
            _db.StatusMessages.Add(initialMessage);

            if (initialMessage.Status == StatusMessageType.Okay)
            {
                incident.End = initialMessage.DateTime;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}