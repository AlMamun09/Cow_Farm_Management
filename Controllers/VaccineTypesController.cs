using Cow_Farm.Data;
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

        public async Task<IActionResult> Index()
        {
            var vaccineTypes = await _context.VaccineTypes.ToListAsync();
            return View(vaccineTypes);
        }

        //Get: VaccineTpes/Create
        public IActionResult Create()
        {
            return View();
        }

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
                return RedirectToAction(nameof(Index));
            }
            return View(vaccineType);
        }
    }
}
