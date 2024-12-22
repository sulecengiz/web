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

    public IActionResult About()
    {
        return View();
    }
}
