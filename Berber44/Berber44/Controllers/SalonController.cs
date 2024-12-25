using Berber44.Models;
using Berber44.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Berber44.Controllers
{
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salon
        public async Task<IActionResult> Index()
        {
            var salonlar = await _context.Salonlar.ToListAsync();
            return View(salonlar);
        }

        // GET: Salon/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        // GET: Salon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Salonlar == null)
            {
                return NotFound();
            }

            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }
            return View(salon);
        }

        // POST: Salon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Salon salon)
        {
            if (id != salon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalonExists(salon.Id))
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
            return View(salon);
        }

        // GET: Salon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Salonlar == null)
            {
                return NotFound();
            }

            var salon = await _context.Salonlar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        // POST: Salon/Delete/5
        // POST: Salon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // İlgili salonu, çalışanlarını ve işlemlerini yükle
            var salon = await _context.Salonlar
                                      .Include(s => s.Calisanlar)
                                      .Include(s => s.Islemler) // Örneğin Islemler tablosu
                                      .FirstOrDefaultAsync(s => s.Id == id);

            foreach (var calisan in salon.Calisanlar)
            {
                if (calisan.Randevular.Any())
                {
                    TempData["ErrorMessage"] = "Bu salon silinemez çünkü çalışanlara bağlı randevular var. Lütfen önce randevuları silin.";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (salon == null)
            {
                TempData["ErrorMessage"] = "Salon bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Bağlı çalışanları kontrol et
            if (salon.Calisanlar.Any())
            {
                TempData["ErrorMessage"] = "Bu salon silinemez çünkü ona bağlı çalışanlar var. Lütfen önce çalışanları silin.";
                return RedirectToAction(nameof(Index));
            }

            // Bağlı işlemleri kontrol et
            if (salon.Islemler.Any()) // Eğer `Islemler` adında bir işlem tablonuz varsa
            {
                TempData["ErrorMessage"] = "Bu salon silinemez çünkü ona bağlı işlemler var. Lütfen önce işlemleri silin.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Silme işlemi
                _context.Salonlar.Remove(salon);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Salon başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction(nameof(Index));
            }
        }



        // GET: Salon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Salonlar == null)
            {
                return NotFound();
            }

            var salon = await _context.Salonlar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        private bool SalonExists(int id)
        {
            return _context.Salonlar.Any(e => e.Id == id);
        }
    }
}
