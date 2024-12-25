using Berber44.Data;
using Berber44.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Berber44.Controllers
{
    public class CalisanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalisanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Calisan
        public async Task<IActionResult> Index()
        {
            // Include related Salon information
            var calisanlar = await _context.Calisanlar
                                           .Include(c => c.Salon)
                                           .ToListAsync();
            return View(calisanlar);
        }

        // GET: Calisan/Create
        public IActionResult Create()
        {
            var salonlar = _context.Salonlar.ToList();

            // If no salons exist, inform the user
            if (!salonlar.Any())
            {
                TempData["ErrorMessage"] = "Önce bir salon eklemeniz gerekiyor.";
                return RedirectToAction("Index", "Salon");
            }

            // Populate dropdown for Salons
            ViewBag.Salonlar = new SelectList(salonlar, "Id", "Ad");
            return View();
        }

        // POST: Calisan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                if (calisan.SalonId == 0 || !_context.Salonlar.Any(s => s.Id == calisan.SalonId))
                {
                    TempData["ErrorMessage"] = "Çalışanı kaydedebilmek için bir salon seçmeniz gerekiyor.";
                    return RedirectToAction("Index", "Salon");
                }

                _context.Calisanlar.Add(calisan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown in case of validation failure
            ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", calisan.SalonId);
            return View(calisan);
        }

        // GET: Calisan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null) return NotFound();

            // Populate dropdown with the selected Salon
            ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", calisan.SalonId);
            return View(calisan);
        }

        // POST: Calisan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Calisan calisan)
        {
            if (id != calisan.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calisan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Calisanlar.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
            }

            // Repopulate dropdown in case of validation failure
            ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", calisan.SalonId);
            return View(calisan);
        }

        // GET: Calisan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var calisan = await _context.Calisanlar
                                        .Include(c => c.Salon)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (calisan == null) return NotFound();

            return View(calisan);
        }

        // POST: Calisan/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan != null)
            {
                _context.Calisanlar.Remove(calisan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
