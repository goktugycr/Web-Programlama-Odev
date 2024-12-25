using Berber44.Data;
using Berber44.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Berber44.Controllers
{
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Randevu (Admin Tüm Randevular)
        public async Task<IActionResult> Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                return RedirectToAction(nameof(Randevularim));
            }

            var randevular = await _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .ToListAsync();

            return View(randevular);
        }

        // GET: Kullanıcı Randevuları
        public async Task<IActionResult> Randevularim()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var userRandevular = await _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .Where(r => r.UserEmail == userEmail)
                .ToListAsync();

            return View(userRandevular);
        }

        // GET: Randevu/Create
        public IActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        // POST: Randevu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu)
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            randevu.UserEmail = userEmail;

            // İşlem seçimine göre ücreti belirle
            var selectedIslem = await _context.Islemler.FindAsync(randevu.IslemId);
            if (selectedIslem != null)
            {
                randevu.Ucret = selectedIslem.Ucret;
            }

            // Çalışanın belirtilen tarihte başka bir randevusu var mı?
            bool calisanMüsaitMi = !_context.Randevular.Any(r =>
                r.CalisanId == randevu.CalisanId &&
                r.TarihSaat == randevu.TarihSaat);

            if (!calisanMüsaitMi)
            {
                ModelState.AddModelError("TarihSaat", "Çalışan bu saatte dolu, lütfen farklı bir saat seçin.");
            }

            if (ModelState.IsValid)
            {
                _context.Randevular.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Randevularim));
            }

            LoadDropDowns(randevu);
            return View(randevu);
        }

        // POST: Randevu/Approve (Sadece Admin)
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                return Unauthorized();
            }

            var randevu = _context.Randevular.Find(id);
            if (randevu == null) return NotFound();

            randevu.Durum = "Onaylandı";
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Randevu/Edit/5 (Sadece Admin)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            LoadDropDowns(randevu);
            return View(randevu);
        }

        // POST: Randevu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Randevu randevu)
        {
            if (id != randevu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata: {ex.Message}");
                }
            }

            LoadDropDowns(randevu);
            return View(randevu);
        }

        // GET: Randevu/Delete/5 (Sadece Admin)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var randevu = await _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (randevu == null) return NotFound();

            return View(randevu);
        }

        // POST: Randevu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                try
                {
                    _context.Randevular.Remove(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Silme işlemi başarısız: {ex.Message}");
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropDowns(Randevu? randevu = null)
        {
            ViewBag.Calisanlar = new SelectList(_context.Calisanlar, "Id", "Ad", randevu?.CalisanId);
            ViewBag.Islemler = new SelectList(_context.Islemler, "Id", "Ad", randevu?.IslemId);
        }

        private bool RandevuExists(int id)
        {
            return _context.Randevular.Any(r => r.Id == id);
        }
    }
}