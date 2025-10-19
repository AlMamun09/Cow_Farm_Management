using Cow_Farm.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cow_Farm.Controllers
{
    public class VaccineTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VaccineTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetVaccineTypes()
        {
            var vaccineTypes = await _context.VaccineTypes
                .Select(v => new
                {
                    v.Id,
                    v.Name,
                    v.VaccineManufacturer,
                    v.Price,
                    v.Description
                })
                .ToListAsync();

            // DataTables expects the array to be in a root property called "data"
            return Json(new { data = vaccineTypes });
        }

        [Authorize]
        //Get: VaccineTpes/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,VaccineManufacturer,Price,Description")] Models.VaccineType vaccineType)
        {
            if (await _context.VaccineTypes.AnyAsync(v => v.Name.ToUpper() == vaccineType.Name.ToUpper()))
            {
                ModelState.AddModelError("Name", "This vaccine name already exists. Please enter a unique name.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(vaccineType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, redirectUrl = Url.Action("Index", "VaccineTypes") });
            }
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors = errors });
        }

        // POST: VaccineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccineType = await _context.VaccineTypes.FindAsync(id);
            if (vaccineType != null)
            {
                _context.VaccineTypes.Remove(vaccineType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Vaccine type deleted successfully." });
            }

            return Json(new { success = false, message = "Error: Vaccine type not found." });
        }
    }
}
