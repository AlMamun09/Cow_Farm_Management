using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cow_Farm.Data;
using Cow_Farm.Models;

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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HealthRecords.Include(h => h.Cow);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber");
            ViewBag.HealthEventTypes = new SelectList(Enum.GetValues(typeof(HealthEventType)));
            return View();
        }

        // POST: HealthRecords/Create OR HealthRecords/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CowId,EventType,RecordDate,Description,Veterinarian")] HealthRecord healthRecord)
        {
            if (ModelState.IsValid)
            {
                if (healthRecord.Id == 0)
                {
                    // This is a NEW record
                    _context.Add(healthRecord);
                }
                else
                {
                    // This is an EXISTING record to update
                    _context.Update(healthRecord);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the dropdowns and return to the form
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber", healthRecord.CowId);
            ViewBag.HealthEventTypes = new SelectList(Enum.GetValues(typeof(HealthEventType)));
            return View(healthRecord);
        }

        // GET: HealthRecords/Edit/5
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
            // Load the dropdown data for the form
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber", healthRecord.CowId);
            ViewBag.HealthEventTypes = new SelectList(Enum.GetValues(typeof(HealthEventType)));

            // IMPORTANT: Return the "Create" view with the existing data
            return View("Create", healthRecord);
        }

        
        // GET: HealthRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: HealthRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var healthRecord = await _context.HealthRecords.FindAsync(id);
            if (healthRecord != null)
            {
                _context.HealthRecords.Remove(healthRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HealthRecordExists(int id)
        {
            return _context.HealthRecords.Any(e => e.Id == id);
        }
    }
}
