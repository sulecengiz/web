using Microsoft.EntityFrameworkCore;

namespace Bookland.Models {
    public static class SeedData {
        public static void EnsurePopulated(IApplicationBuilder app) {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            // Migration'ları kontrol et ve uygula
            if (context.Database.GetPendingMigrations().Any()) {
                context.Database.Migrate();
            }

            // User tablosunda veri yoksa yeni kullanıcı ekle
            if (!context.Users.Any()) {
                context.Users.AddRange(
                    new User {
                        Username = "Jane Doe",
                        Email = "jane.doe@example.com",
                        Password = "1234",
                        Phone = "123-456-7890"
                    },
                    new User {
                        Username = "sulecengiz",
                        Email = "sule@mail.com",
                        Password = "1234",
                        Phone = "123-456-7899"
                    }
                );

                // Kullanıcı verilerini kaydet
                context.SaveChanges();
            }

            // Product tablosunda veri yoksa yeni ürünler ekle
            if (!context.Products.Any()) {
                context.Products.AddRange(
                    new Product {
                        Title = "Sapiens",
                        Author = "Yuval Noah Harari",
                        Price = 49.99m,
                        ImageUrl = "/images/sapiens.jpg"
                    },
                    new Product {
                        Title = "1984",
                        Author = "George Orwell",
                        Price = 29.99m,
                        ImageUrl = "/images/1984.jpg"
                    },
                    new Product {
                        Title = "Dune",
                        Author = "Frank Herbert",
                        Price = 39.99m,
                        ImageUrl = "/images/dune.jpg"
                    }
                );

                // Ürün verilerini kaydet
                context.SaveChanges();
            }
        }
    }
}
