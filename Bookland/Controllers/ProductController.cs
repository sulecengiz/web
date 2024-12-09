using Microsoft.AspNetCore.Mvc;
using Bookland.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ProductController : Controller
{
    private readonly StoreDbContext _context;

    public ProductController(StoreDbContext context)
    {
        _context = context;
    }
   [HttpPost]
    public IActionResult AddToFavorites(int productId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcıyı bulma

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Login", "Account"); // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
        }

        int userId;
        if (!int.TryParse(userIdString, out userId))
        {
            return BadRequest("Geçersiz kullanıcı ID"); // Kullanıcı ID'si geçersizse hata mesajı döndür
        }

        var favorite = new Favorite
        {
            UserId = userId, // Kullanıcı ID'si doğru şekilde atandı
            ProductId = productId
        };

        _context.Favorites.Add(favorite);
        _context.SaveChanges();

        return RedirectToAction("Index"); // Favoriye ekledikten sonra kitaplar sayfasına yönlendir
    }

    public IActionResult Favorites()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini al

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Login", "Account"); // Kullanıcı giriş yapmadıysa login sayfasına yönlendir
        }

        int userId;
        if (!int.TryParse(userIdString, out userId)) // string'i int'ye dönüştürme
        {
            return BadRequest("Geçersiz kullanıcı ID"); // Kullanıcı ID'si geçersizse hata mesajı döndür
        }

        var favorites = _context.Favorites
                                .Where(f => f.UserId == userId)
                                .Include(f => f.Product) // Kitap bilgilerini de al
                                .ToList();

        return View(favorites); // Favorileri görüntüle
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
