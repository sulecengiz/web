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
    [HttpGet]
    public IActionResult Create(){
        ViewBag.Categories = _context.ProductCategories.ToList();
        return View();
    }
    [HttpPost]
    public IActionResult Create(Product book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }
    public IActionResult Index() {
    var products = _context.Products.ToList();
    return View(products);
    }

    // UPDATE: Edit an existing book
    public IActionResult Edit(int id)
    {
        var book = _context.Products.Find(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Title,Author,Category")] Product book)
    {
        if (id != book.ProductID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductID == book.ProductID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // DELETE: Delete a book
    public IActionResult Delete(int id)
    {
        var book = _context.Products.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Products.Remove(book);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
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
