using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookland.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Linq;

namespace Bookland.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreDbContext _context;

        public AccountController(StoreDbContext context)
        {
            _context = context;
        }

        // Kullanıcı girişi ve kayıt işlemleri

        // Login GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

       // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Kullanıcıyı oturum açtıktan sonra hatırlamak için
                };

                // Asenkron işlem için await ekliyoruz
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Başarılı girişte ana sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Giriş hatalı ise tekrar login sayfasına yönlendir
            ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }


        // Register GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Register POST
        [HttpPost]
        public IActionResult Register(string Email,string Username, string Password, string ConfirmPassword)
        {
            if (_context.Users.Any(u => u.Username == Username))
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı adı zaten mevcut.");
                return View();
            }
            if (Password != ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Şifreler eşleşmiyor.";
                return View();  // Hata varsa tekrar formu gösterir
            }

            // User objesini oluşturuyoruz
            var user = new User
            {
                Username = Username,
                Email = Email,
                Password = Password
            };

            // Veritabanına ekleme işlemi
            _context.Users.Add(user);
            _context.SaveChanges(); // Veritabanına kaydetme

            return RedirectToAction("Index", "Home");
        }


        // Çıkış işlemi
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
