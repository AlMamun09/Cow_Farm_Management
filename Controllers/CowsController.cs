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
            return View(await _context.Cow.ToListAsync());
        }

        // GET: Cows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cow == null)
            {
                return NotFound();
            }

            return View(cow);
        }

        // GET: Cows/Create
        public IActionResult Create()
        {
            ViewBag.Genders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(Status)));
            return View();
        }

        // POST: Cows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Breed,BirthDate,Gender,Status,Weight")] Cow cow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cow);
        }

        // GET: Cows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cow.FindAsync(id);
            if (cow == null)
            {
                return NotFound();
            }
            return View(cow);
        }

        // POST: Cows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Breed,BirthDate,Gender,Status,Weight")] Cow cow)
        {
            if (id != cow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CowExists(cow.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cow);
        }

        // GET: Cows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cow
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
            var cow = await _context.Cow.FindAsync(id);
            if (cow != null)
            {
                _context.Cow.Remove(cow);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CowExists(int id)
        {
            return _context.Cow.Any(e => e.Id == id);
        }
    }
}
