using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bookland.Models;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View("login");
    }

    [HttpPost]
    public async Task<IActionResult> Register(string userName, string email, string password)
    {
        if (ModelState.IsValid)
        {
            // Create a new ApplicationUser object
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            // Create the user with the provided password
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Assign a role to the user (you can change "User" to a different role if needed)
                await _userManager.AddToRoleAsync(user, "User");

                // Automatically sign in the user after successful registration
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect to the home page or another page as per your requirements
                return RedirectToAction("Index", "Home");
            }

            // If there were errors in the registration, add them to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If the model is invalid, return to the registration view with the model state
        return View();
    }
}
