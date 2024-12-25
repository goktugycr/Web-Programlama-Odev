using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Berber44.Data; // Veritabanı bağlamı için
using Berber44.Models; // Modeller için
using Microsoft.AspNetCore.Authorization;

namespace Berber44.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor ile DbContext enjekte ediliyor
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin ana sayfası
        public IActionResult Index()
        {
            return View();
        }

        // Kullanıcı listesi
        [HttpGet]
        public IActionResult UserList()
        {
            var users = _context.Users.ToList(); // Tüm kullanıcıları veritabanından getir
            return View(users); // View'a kullanıcı listesini gönder
        }

        // Kullanıcı düzenleme sayfası (GET)
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id); // ID ile kullanıcıyı bul
            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }
            return View(user); // Kullanıcıyı düzenleme sayfasına gönder
        }

        // Kullanıcı düzenleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user); // Kullanıcıyı güncelle
                _context.SaveChanges(); // Veritabanına değişiklikleri kaydet
                return RedirectToAction("UserList"); // Listeye geri dön
            }
            return View(user); // Hatalıysa aynı sayfayı göster
        }

        // Kullanıcı silme işlemi
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id); // ID ile kullanıcıyı bul
            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }
            _context.Users.Remove(user); // Kullanıcıyı sil
            _context.SaveChanges(); // Değişiklikleri kaydet
            return RedirectToAction("UserList"); // Listeye geri dön
        }
        public IActionResult RandevuYonetimi()
        {
            return View();
        }
    }
    

}
