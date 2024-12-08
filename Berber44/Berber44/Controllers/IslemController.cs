using Berber44.Data;
using Berber44.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Berber44.Controllers
{
    public class IslemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IslemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Islem
        public async Task<IActionResult> Index()
        {
            // İşlemleri ilgili salon bilgisiyle birlikte getir
            var islemler = await _context.Islemler.Include(i => i.Salon).ToListAsync();
            return View(islemler);
        }

        // GET: Islem/Create
        public IActionResult Create()
        {
            var salonlar = _context.Salonlar.ToList();
            if (!salonlar.Any())
            {
                TempData["ErrorMessage"] = "Önce bir salon eklemeniz gerekiyor.";
                return RedirectToAction("Index", "Salonlar");
            }
            // Salonlar dropdown için yükleniyor
            ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }




        // POST: Islem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Islem islem)
        {
            // ModelState'in geçerliliğini kontrol et
            if (ModelState.IsValid)
            {
                // Veritabanına ekle ve kaydet
                _context.Islemler.Add(islem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Eğer doğrulama başarısız olursa tekrar dropdown'ı yükle
            ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", islem.SalonId);
            return View(islem);
        }

        // GET: Islem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var islem = await _context.Islemler
                .Include(i => i.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (islem == null)
            {
                return NotFound();
            }

            return View(islem);
        }

        // POST: Islem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var islem = await _context.Islemler.FindAsync(id);
            if (islem != null)
            {
                _context.Islemler.Remove(islem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
