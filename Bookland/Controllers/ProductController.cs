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
    public IActionResult Create()
    {
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
            return RedirectToAction("Index", "Home");
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

    public IActionResult AddToCart(long productId)
    {
        string userId = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;

        // Sepetteki ürünü kontrol et
        var existingItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductID == productId);

        if (existingItem != null)
        {
            // Eğer ürün zaten sepette varsa, miktarı artır
            existingItem.Quantity++;
        }
        else
        {
            // Eğer ürün yoksa, yeni bir CartItem ekle
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                var newCartItem = new CartItem
                {
                    UserId = userId,
                    ProductID = product.ProductID,
                    Title = product.Title,
                    Author = product.Author,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                };

                _context.CartItems.Add(newCartItem);
            }
        }

        _context.SaveChanges();
        return RedirectToAction("Cart");
    }



    // Sepeti görüntüle
    public IActionResult Cart()
    {
        string userId = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;

        // Kullanıcının sepetteki tüm öğelerini al
        var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();

        if (cartItems.Count == 0)
        {
            return View(cartItems); // Sepet boşsa, boş sepet sayfasını göster
        }

        return View(cartItems); // Sepetteki öğeleri görüntüle
    }


    // Sepetten ürün çıkar
    [HttpPost]

    public IActionResult RemoveFromCart(int cartItemId)
    {
        string userId = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;

        // Sepetteki öğeyi bul
        var cartItem = _context.CartItems.FirstOrDefault(c => c.Id == cartItemId && c.UserId == userId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        return RedirectToAction("Cart");
    }




    // DELETE: Delete a book
    public IActionResult Delete(long ProductID)
{
    var book = _context.Products.Find(ProductID);
    
    if (book == null)
    {
        // Eğer ürün bulunamazsa uygun bir hata mesajı dönebilir veya hata sayfasına yönlendirebilirsiniz
        return NotFound("Ürün bulunamadı.");
    }

    _context.Products.Remove(book);
    _context.SaveChanges();

    return RedirectToAction("Index", "Home");
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
    string userId = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;

    // Ürün bilgilerini al
    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
    if (product == null)
    {
        return NotFound(); // Ürün bulunamadı
    }

    // Zaten favorilerde olup olmadığını kontrol et
    var existingFavorite = await _context.Favorites
        .FirstOrDefaultAsync(f => f.ProductID == productId && f.UserId == userId);

    if (existingFavorite == null)
    {
        // Yeni favori ekle
        var favorite = new Favorite
        {
            UserId = userId,
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
    string userId = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;

    // Kullanıcıya ait favorileri getir
    var favorites = await _context.Favorites
        .Where(f => f.UserId == userId)
        .ToListAsync();

    return View(favorites);
}

[HttpPost]
public async Task<IActionResult> RemoveFromFavorites(int productId)
{
    string userId = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;

    var favorite = await _context.Favorites
        .FirstOrDefaultAsync(f => f.ProductID == productId && f.UserId == userId);

    if (favorite != null)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction("Favorites", "Product");
}






}
