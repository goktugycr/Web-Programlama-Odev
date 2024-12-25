using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Berber44.Data;
using Berber44.Models;

namespace Berber44.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandevuApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RandevuApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RandevuApi/GetAll - Tüm randevuları listele
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var randevular = await _context.Randevus
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .Select(r => new
                {
                    r.Id,
                    CalisanAdSoyad = r.Calisan != null ? r.Calisan.Ad + " " + r.Calisan.Soyad : "Atanmamış",
                    IslemAd = r.Islem != null ? r.Islem.Ad : "Tanımlanmamış",
                    IslemUcret = r.Islem != null ? r.Islem.Ucret : 0,
                    KullaniciEmail = r.UserEmail,
                    TarihSaat = r.TarihSaat,
                    Durum = r.Durum
                })
                .ToListAsync();

            return Ok(randevular);
        }

        // DELETE: api/RandevuApi/5 - Randevu sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRandevu(int id)
        {
            var randevu = await _context.Randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound(new { Message = "Randevu bulunamadı." });
            }

            _context.Randevus.Remove(randevu);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
