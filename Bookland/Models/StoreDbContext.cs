using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Bookland.Models{
    public class StoreDbContext : DbContext
    {        
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options) { }
        
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    }
}