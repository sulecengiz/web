using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookland.Models
{
    public static class SeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Migration'ları kontrol et ve uygula
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            // Kullanıcı rolü oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Örnek bir kullanıcı ekleyin
            if (await userManager.FindByEmailAsync("admin@bookland.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    Username = "admin",
                    Email = "admin@bookland.com",
                    Phone = "123-456-7890"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            if (await userManager.FindByEmailAsync("sule@mail.com") == null)
            {
                var suleUser = new ApplicationUser
                {
                    Username = "sulecengiz",
                    Email = "sule@mail.com",
                    Phone = "123-456-7899",
                    PasswordHash = "123456"
                };

                var result = await userManager.CreateAsync(suleUser, "User1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(suleUser, "User");
                }
            }

            // Product ve Category tablolarını kontrol et
            if (!context.Products.Any())
            {
                // Önce Category tablosuna kategori ekleyelim
                var fictionCategory = new ProductCategory { Name = "Fiction", BookCount = 0 };
                var nonFictionCategory = new ProductCategory { Name = "Non-Fiction", BookCount = 0 };

                await context.ProductCategories.AddRangeAsync(fictionCategory, nonFictionCategory);
                await context.SaveChangesAsync();

                // Books tablosuna kitaplar ekleyelim
                await context.Products.AddRangeAsync(
                    new Product
                    {
                        Title = "Sapiens",
                        Author = "Yuval Noah Harari",
                        Price = 49.99m,
                        ImageUrl = "/images/sapiens.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product
                    {
                        Title = "1984",
                        Author = "George Orwell",
                        Price = 29.99m,
                        ImageUrl = "/images/1984.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product
                    {
                        Title = "Dune",
                        Author = "Frank Herbert",
                        Price = 39.99m,
                        ImageUrl = "/images/dune.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
