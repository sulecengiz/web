using Microsoft.AspNetCore.Mvc;
using Bookland.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult Index()
{
    var products = _context.Products.ToList(); // Ürünleri al
    return View(products); // List<Product> tipinde veri gönder
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

        return View("Search", books); // Index görünümünü yeniden kullan
    }



    
[HttpPost]
public async Task<IActionResult> AddToFavorites(long productId)
{
    // Ürün bilgilerini al
    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
    if (product == null)
    {
        return NotFound(); // Ürün bulunamadı
    }

    // Zaten favorilerde olup olmadığını kontrol et
    var existingFavorite = await _context.Favorites
        .FirstOrDefaultAsync(f => f.ProductID == productId);

    if (existingFavorite == null)
    {
        // Yeni favori ekle
        var favorite = new Favorite
        {
            ProductID = product.ProductID,
            Title = product.Title,
            Author = product.Author,
            Price = product.Price,
            ImageUrl = product.ImageUrl
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();
    }

    // Favoriler sayfasına yönlendir
    return RedirectToAction("Favorites", "Product");
}




   public async Task<IActionResult> Favorites()
{
    // Veritabanından favorileri al (UserID olmadan, sadece ProductID'yi kullanarak)
    var favorites = await _context.Favorites.ToListAsync(); // Tüm favorileri listele

    return View(favorites);
}


    

   

    // Favorilerden bir ürünü kaldırma
    [HttpPost]
    public async Task<IActionResult> RemoveFromFavorites(int productId)
    {
        
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f =>  f.ProductID == productId);

        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        // Favoriler sayfasına geri yönlendir
        return RedirectToAction("Favorites", "Product");
    }





}
