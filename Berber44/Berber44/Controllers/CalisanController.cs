using Berber44.Data;
using Berber44.Models;
using Microsoft.AspNetCore.Mvc;
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
            var calisanlar = await _context.Calisanlar.ToListAsync();
            return View(calisanlar);
        }

        // GET: Calisan/Create
        public IActionResult Create()
        {
            var salonlar = _context.Salonlar.ToList();

            if (!salonlar.Any())
            {
                TempData["ErrorMessage"] = "Önce bir salon eklemeniz gerekiyor.";
                return RedirectToAction("Index", "Salonlar");
            }

            ViewBag.Salonlar = salonlar;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calisan calisan)
        {
            Console.WriteLine("Create POST method invoked.");

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState is valid.");
                _context.Calisanlar.Add(calisan);
                await _context.SaveChangesAsync();
                Console.WriteLine("Calisan added to the database.");
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("ModelState is invalid.");
            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(calisan);
        }


        // GET: Calisan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Calisanlar == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null)
            {
                return NotFound();
            }
            return View(calisan);
        }

        // POST: Calisan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Calisan calisan)
        {
            if (id != calisan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calisan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Calisanlar.Any(e => e.Id == calisan.Id))
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
            return View(calisan);
        }

        // GET: Calisan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Calisanlar == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisanlar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        // POST: Calisan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan != null)
            {
                _context.Calisanlar.Remove(calisan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
