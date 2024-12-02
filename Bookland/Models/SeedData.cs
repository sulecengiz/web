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
                        // Phone alanı isteğe bağlı bırakıldı
                    },
                    new User {
                        Username = "sulecengiz",
                        Email = "sule@mail.com",
                        Password = "1234", // İsteğe bağlı olarak değer atanabilir
                        Phone = "123-456-7899"
                    }
                );

                // Veritabanına kaydet
                context.SaveChanges();
            }
        }
    }
}
