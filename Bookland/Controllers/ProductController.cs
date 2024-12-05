using Microsoft.AspNetCore.Mvc;
using Bookland.Models;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly StoreDbContext _context;

    public ProductController(StoreDbContext context)
    {
        _context = context;
    }
    public IActionResult Index() {
    var products = _context.Products.ToList();
    return View(products);
    }

    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return RedirectToAction("Index", "Home");
        }

        var books = await _context.Products
            .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
            .ToListAsync();

        return View("Index", books); // Index görünümünü yeniden kullan
    }
}
