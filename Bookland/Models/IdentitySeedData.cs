using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookland.Models
{
    public static class IdentitySeedData
    {
        private const string adminRole = "Admin";
        private const string userRole = "User";

        private const string adminUser = "Admin";
        private const string adminPassword = "AdminPassword123$";
        private const string regularUser = "User";
        private const string regularPassword = "UserPassword123$";

        public static async Task EnsurePopulated(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Admin rolünü oluştur
            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // User rolünü oluştur
            if (await roleManager.FindByNameAsync(userRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(userRole));
            }

            // Admin kullanıcısını oluştur
            var admin = await userManager.FindByNameAsync(adminUser);
            if (admin == null)
            {
                admin = new IdentityUser(adminUser)
                {
                    Email = "admin@yourapp.com",
                    UserName = adminUser,
                    PhoneNumber = "1234567890"
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
            }

            // Regular kullanıcıyı oluştur
            var regular = await userManager.FindByNameAsync(regularUser);
            if (regular == null)
            {
                regular = new IdentityUser(regularUser)
                {
                    Email = "user@yourapp.com",
                    UserName = regularUser,
                    PhoneNumber = "0987654321"
                };
                var result = await userManager.CreateAsync(regular, regularPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regular, userRole);
                }
            }
        }
    }
}
