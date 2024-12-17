using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bookland.Models;
using System.Threading.Tasks;
using Bookland.ViewModels;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login(string ReturnUrl)
    {
        ViewData["ReturnUrl"] = ReturnUrl;
        return View();
    }

    
   
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
{
    // Kullanıcıyı adı ile bulmaya çalış
    var user = await _userManager.FindByNameAsync(model.UserName);

    // Kullanıcıyı bulamazsa hata mesajı ekle ve giriş sayfasına dön
    if (user == null)
    {
        ViewBag.ErrorMessage = "Geçersiz kullanıcı adı.";
        return View(model); // Login sayfasını tekrar göster
    }

    // Kullanıcı giriş bilgilerini kontrol et
    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

    // Giriş başarılı değilse hata mesajı ekle ve giriş sayfasına dön
    if (!result.Succeeded)
    {
        ViewBag.ErrorMessage = "Geçersiz şifre veya kullanıcı adı.";
        return View(model); // Login sayfasını tekrar göster
    }

    // Başarılı giriş sonrası yönlendirme
    return RedirectToAction("Index", "Home");
}




    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // Profil sayfası action'ı
    [HttpGet]
    [HttpGet]
public async Task<IActionResult> Profile()
{
    // Giriş yapmış kullanıcıyı al
    var user = await _userManager.GetUserAsync(User);

    if (user == null)
    {
        // Eğer kullanıcı giriş yapmamışsa, login sayfasına yönlendir
        return RedirectToAction("Login", "Account");
    }

    // Profil bilgilerini taşıyan model oluştur
    var model = new UserProfileViewModel
    {
        UserName = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber
    };

    // Profil bilgilerini view'a gönder
    return View(model);
}

}
