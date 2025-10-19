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
    public class CowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cows
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCows()
        {
            var cows = await _context.Cows
                .Select(c => new
                {
                    c.Id,
                    c.TagNumber,
                    c.Name,
                    c.Breed,
                    BirthDate = c.BirthDate.ToShortDateString(), // Format the date here
                    Gender = c.Gender.ToString(), // Convert enum to string
                    Status = c.Status.ToString()  // Convert enum to string
                })
                .ToListAsync();

            return Json(new { data = cows });
        }

        // GET: Cows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cow == null)
            {
                return NotFound();
            }

            return View(cow);
        }


        // GET: Cows/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Genders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(CowStatus)));
            ViewBag.Dams = new SelectList(await _context.Cows.Where(c => c.Gender == Gender.Female).ToListAsync(), "Id", "TagNumber");
            ViewBag.Sires = new SelectList(await _context.Cows.Where(c => c.Gender == Gender.Male).ToListAsync(), "Id", "TagNumber");

            return View();
        }

        // POST: Cows/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TagNumber,Name,Breed,BirthDate,Gender,Status,DamId,SireId")] Cow cow)
        {
            if (cow.Id == 0)
            {
                ModelState.Remove(nameof(Cow.TagNumber));
            }

            if (ModelState.IsValid)
            {
                bool isNew = cow.Id == 0;
                if (isNew)
                {
                    // Logic to generate new TagNumber
                    var lastCow = await _context.Cows.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                    int nextNumber = 1;
                    if (lastCow != null && !string.IsNullOrEmpty(lastCow.TagNumber) && lastCow.TagNumber.StartsWith("CID-"))
                    {
                        string lastNumberStr = lastCow.TagNumber.Substring(4);
                        if (int.TryParse(lastNumberStr, out int lastNumber))
                        {
                            nextNumber = lastNumber + 1;
                        }
                    }
                    cow.TagNumber = $"CID-{nextNumber:D4}";
                    _context.Add(cow);
                }
                else
                {
                    _context.Update(cow);
                }
                await _context.SaveChangesAsync();

                // Instead of redirecting, return a JSON success response
                TempData["SuccessMessage"] = isNew ? "New cow created successfully!" : "Cow record updated successfully!";
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Cows") });
            }

            // If validation fails, return a JSON error response with the validation messages
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return Json(new { success = false, errors = errors });
        }

        // GET: Cows/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows.FindAsync(id);
            if (cow == null)
            {
                return NotFound();
            }

            ViewBag.Genders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(CowStatus)));
            ViewBag.Dams = new SelectList(await _context.Cows.Where(c => c.Gender == Gender.Female && c.Id != id).ToListAsync(), "Id", "TagNumber", cow.DamId);
            ViewBag.Sires = new SelectList(await _context.Cows.Where(c => c.Gender == Gender.Male && c.Id != id).ToListAsync(), "Id", "TagNumber", cow.SireId);

            return View("Create", cow);
        }


        // POST: Cows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cow = await _context.Cows.FindAsync(id);
            if (cow != null)
            {
                _context.Cows.Remove(cow);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cow record deleted successfully." });
            }
            return Json(new { success = false, message = "Error: Cow not found." });
        }
    }
}
