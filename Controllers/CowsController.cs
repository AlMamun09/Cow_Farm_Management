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
    public class CowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cows
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cows.ToListAsync());
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Genders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(CowStatus)));
            ViewBag.Dams = new SelectList(await _context.Cows.Where(c => c.Gender == Gender.Female).ToListAsync(), "Id", "TagNumber");
            ViewBag.Sires = new SelectList(await _context.Cows.Where(c => c.Gender == Gender.Male).ToListAsync(), "Id", "TagNumber");

            return View();
        }

        // POST: Cows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                if (cow.Id == 0)
                {
                    var lastCow = await _context.Cows
                        .OrderByDescending(c => c.Id)
                        .FirstOrDefaultAsync();

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
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(CowStatus)));
            return View(cow);
        }

        // GET: Cows/Edit/5
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


        // GET: Cows/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Cows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cow = await _context.Cows.FindAsync(id);
            if (cow != null)
            {
                _context.Cows.Remove(cow);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CowExists(int id)
        {
            return _context.Cows.Any(e => e.Id == id);
        }
    }
}
