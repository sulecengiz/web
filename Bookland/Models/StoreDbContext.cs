using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Bookland.Models{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {        
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options) { }
        
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    }
}