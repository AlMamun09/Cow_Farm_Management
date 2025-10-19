using Cow_Farm.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetVaccinations()
        {
            var vaccinations = await _context.Vaccinations
                .Include(v => v.Cow)
                .Include(v => v.VaccineType)
                .Select(v => new
                {
                    v.Id,
                    CowTagNumber = v.Cow != null ? v.Cow.TagNumber : "N/A",
                    VaccineName = v.VaccineType != null ? v.VaccineType.Name : "N/A",
                    DateGiven = v.DateGiven.ToShortDateString(),
                    NextDueDate = v.NextDueDate.HasValue ? v.NextDueDate.Value.ToShortDateString() : "N/A"
                })
                .ToListAsync();

            return Json(new { data = vaccinations });
        }

        [Authorize]
        // GET: Vaccinations/Create
        public IActionResult Create()
        {
            ViewData["CowId"] = new SelectList(_context.Cows, "Id", "TagNumber");
            ViewData["VaccineTypeId"] = new SelectList(_context.VaccineTypes, "Id", "Name");
            return View();
        }

        [Authorize]
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

        // POST: Vaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccination = await _context.Vaccinations.FindAsync(id);
            if (vaccination != null)
            {
                _context.Vaccinations.Remove(vaccination);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Vaccination record deleted successfully." });
            }

            return Json(new { success = false, message = "Error: Record not found." });
        }
    }
}
