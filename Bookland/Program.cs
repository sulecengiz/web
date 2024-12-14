using Microsoft.EntityFrameworkCore;
using Bookland.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext'i kaydet
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity'yi ekleyin
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();

// Authentication ve Authorization servislerini ekle
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/login"; // Kullanıcı giriş yapmamışsa yönlendirilecek yol
        options.LogoutPath = "/Account/login"; // Çıkış için yol
        // options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz erişim için yönlendirilecek yol (isteğe bağlı)
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Seed data burada çalıştırılır
await SeedData.EnsurePopulated(app);

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
