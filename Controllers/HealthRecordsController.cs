using Cow_Farm.Data;
using Cow_Farm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cow_Farm.Controllers
{
    public class HealthRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HealthRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HealthRecords
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHealthRecords()
        {
            var healthRecords = await _context.HealthRecords
                .Include(h => h.Cow)
                .Select(h => new
                {
                    h.Id,
                    CowTagNumber = h.Cow != null ? h.Cow.TagNumber : "N/A",
                    CowName = h.Cow != null ? h.Cow.Name : "N/A",
                    EventType = h.EventType.ToString(),
                    RecordDate = h.RecordDate.ToShortDateString(),
                    h.Description,
                    h.Veterinarian
                })
                .ToListAsync();

            return Json(new { data = healthRecords });
        }

        // GET: HealthRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthRecord = await _context.HealthRecords
                .Include(h => h.Cow)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (healthRecord == null)
            {
                return NotFound();
            }

            return View(healthRecord);
        }

        // GET: HealthRecords/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber");
            ViewBag.HealthEventTypes = new SelectList(Enum.GetValues(typeof(HealthEventType)));
            return View();
        }

        // POST: HealthRecords/Create OR HealthRecords/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,CowId,EventType,RecordDate,Description,Veterinarian")] HealthRecord healthRecord)
        {
            if (ModelState.IsValid)
            {
                bool isNew = healthRecord.Id == 0;
                if (isNew)
                {
                    _context.Add(healthRecord);
                }
                else
                {
                    _context.Update(healthRecord);
                }
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = isNew ? "New health record created successfully!" : "Health record updated successfully!";
                return Json(new { success = true, redirectUrl = Url.Action("Index", "HealthRecords") });
            }

            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return Json(new { success = false, errors = errors });
        }

        // GET: HealthRecords/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthRecord = await _context.HealthRecords.FindAsync(id);
            if (healthRecord == null)
            {
                return NotFound();
            }
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber", healthRecord.CowId);
            ViewBag.HealthEventTypes = new SelectList(Enum.GetValues(typeof(HealthEventType)));

            return View("Create", healthRecord);
        }

        // POST: HealthRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var healthRecord = await _context.HealthRecords.FindAsync(id);
            if (healthRecord != null)
            {
                _context.HealthRecords.Remove(healthRecord);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Health record deleted successfully." });
            }

            return Json(new { success = false, message = "Error: Record not found." });
        }

        private bool HealthRecordExists(int id)
        {
            return _context.HealthRecords.Any(e => e.Id == id);
        }
    }
}
