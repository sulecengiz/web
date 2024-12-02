using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Bookland.Models
{
    public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            // Bağlantı dizesini okuma
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);

            return new StoreDbContext(optionsBuilder.Options);
        }
    }
}
