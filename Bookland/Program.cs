using Microsoft.EntityFrameworkCore;
using Bookland.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext'i kaydet
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication ve Authorization servislerini ekle
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Kullanıcı giriş yapmamışsa yönlendirilecek yol
        /* options.LogoutPath = "/Account/Logout"; // Çıkış için yol */
        // options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz erişim için yönlendirilecek yol (isteğe bağlı)
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Seed data burada çalıştırılır
SeedData.EnsurePopulated(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // The default HSTS value is 30 days.
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware'lerin doğru sıralamada olduğundan emin olun
app.UseAuthentication(); // Kullanıcı kimlik doğrulaması
app.UseAuthorization();  // Kullanıcı yetkilendirme

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
