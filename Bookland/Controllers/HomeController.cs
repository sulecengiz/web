using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Bookland.Models;

namespace Bookland.Controllers;

public class HomeController : Controller
{
    private readonly StoreDbContext context;

    public HomeController(StoreDbContext _context)
    {
        context = _context;
    }
    public async Task<IActionResult> Index()
    {
        // Popülerlik sırasına göre kitapları çek
        var books = await context.Products
            .OrderByDescending(b => b.Popularity) // Popülerlik sırasına göre
            .ToListAsync();

        return View(books);
    }
   // Ana sayfa
    public IActionResult Login()
    {
        // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        // Eğer kullanıcı giriş yapmışsa, ana sayfayı göster
        return View("Index", "Home");
    }

}
