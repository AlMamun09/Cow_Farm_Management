using Microsoft.AspNetCore.Mvc;
using Cow_Farm.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Cow_Farm.Controllers
{
    public class VaccinationsController : Controller
    {
        public readonly ApplicationDbContext _context;
        public VaccinationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vaccinations
        public async Task<IActionResult> Index()
        {
            var vaccinations = await _context.Vaccinations
                .Include(v => v.Cow)
                .Include(v => v.VaccineType)
                .ToListAsync();

            return View(vaccinations);
        }

        // GET: Vaccinations/Create
        public IActionResult Create()
        {
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber");
            ViewData["VaccineTypeId"] = new SelectList(_context.VaccineTypes, "Id", "Name");
            return View();
        }

        // POST: Vaccinations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CowId,VaccineTypeId,DateGiven,NextDueDate")] Models.Vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaccination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber", vaccination.CowId);
            ViewData["VaccineTypeId"] = new SelectList(_context.VaccineTypes, "Id", "Name", vaccination.VaccineTypeId);
            return View(vaccination);
        }
    }
}
