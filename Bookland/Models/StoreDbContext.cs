using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Bookland.Models{
    public class StoreDbContext : DbContext
    {        
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options) { }
        
        public DbSet<Product> Products => Set<Product>();
         public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<CartItem> CartItems { get; set; }

    }
}