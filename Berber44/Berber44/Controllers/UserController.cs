using Berber44.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Berber44.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Panel()
        {
            // Kullanıcı rolü Claims üzerinden kontrol edilir
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult RandevuList()
        {
            // Kullanıcı e-postası Claims üzerinden alınır
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının kendi randevularını getir
            var randevular = _context.Randevular
                .Where(r => r.UserEmail == userEmail)
                .ToList();

            return View(randevular);
        }
    }
}
