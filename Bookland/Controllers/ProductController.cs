using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookland.Models;
public class ProductController : Controller{
    private readonly StoreDbContext context;
    public ProductController(StoreDbContext _context){
        context = _context;
    }
    public ActionResult Index(){
        return View(context.Products);
    }
    [HttpGet]
    public ActionResult Create(){return View();}
    [HttpPost]
    public async Task<ActionResult> Create(Product product){
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

}
