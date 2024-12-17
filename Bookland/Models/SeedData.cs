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

            // Product tablosunda veri yoksa yeni ürünler ekle
            if (!context.Products.Any()) {
                // Önce Category tablosuna kategori ekleyelim
                var fictionCategory = new ProductCategory { Name = "Fiction", BookCount = 0 };
                var nonFictionCategory = new ProductCategory { Name = "Non-Fiction", BookCount = 0 };

                context.ProductCategories.AddRange(fictionCategory, nonFictionCategory);
                context.SaveChanges();

                // Books tablosuna kitaplar ekleyelim
                context.Products.AddRange(
                    new Product {
                        Title = "Sapiens",
                        Author = "Yuval Noah Harari",
                        Price = 149.99m,
                        ImageUrl = "/images/sapiens.jpg",
                        CategoryId = fictionCategory.ProductCategoryID  // Kategoriyi bağla
                    },
                    new Product {
                        Title = "1984",
                        Author = "George Orwell",
                        Price = 129.99m,
                        ImageUrl = "/images/1984.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Dune",
                        Author = "Frank Herbert",
                        Price = 239.99m,
                        ImageUrl = "/images/dune.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Serenad",
                        Author = "Zülfü Livaneli",
                        Price = 139.99m,
                        ImageUrl = "/images/serenad.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Kürk Mantolu Madonna",
                        Author = "Sabahattin Ali",
                        Price = 99.99m,
                        ImageUrl = "/images/kurk_mantolu.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Simyacı",
                        Author = "Paulo Coelho",
                        Price = 119.99m,
                        ImageUrl = "/images/simyaci.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Savaş ve Barış",
                        Author = "Lev Tolstoy",
                        Price = 249.99m,
                        ImageUrl = "/images/savas_ve_baris.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Fahrenheit 451",
                        Author = "Ray Bradbury",
                        Price = 239.99m,
                        ImageUrl = "/images/fahrenheit_451.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    },
                    new Product {
                        Title = "Kayıp Sembol",
                        Author = "Dan Brown",
                        Price = 139.99m,
                        ImageUrl = "/images/kayip_sembol.jpg",
                        CategoryId = fictionCategory.ProductCategoryID
                    }  
                );
    
                context.SaveChanges();
            }

        }
    }
}
