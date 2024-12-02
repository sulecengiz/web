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

   // Ana sayfa
    public IActionResult Index()
    {
        // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        // Eğer kullanıcı giriş yapmışsa, ana sayfayı göster
        return View();
    }

}
