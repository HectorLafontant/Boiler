using Microsoft.AspNetCore.Mvc;

using Boiler.Models;
using Microsoft.EntityFrameworkCore;
using Boiler.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

using Boiler.DTOs;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Boiler.Controllers
{
    public class LogInController : Controller
    {
        private readonly BoilerContext _context;
        public LogInController(BoilerContext boilerContext) {
            _context = boilerContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            Account? emailAlreadyExists = await _context.Accounts
                                        .Where(account =>
                                        account.Email == registerViewModel.Email!.ToLower())
                                        .FirstOrDefaultAsync();
            if (emailAlreadyExists != null)
            {
                ViewData["ErrorMessage"] = "The E-mail already has an account.";
                return View();
            }

            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                ViewData["ErrorMessage"] = "Passwords don't match.";
                return View();
            }

            Account account = new Account()
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email!.ToLower(),
                Password = registerViewModel.Password
            };

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            if (account.Id != 0) return RedirectToAction("LogIn", "LogIn");

            ViewData["ErrorMessage"] = "Can't create account.";
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (User.Identity!.IsAuthenticated && UserId.GetId() != 0) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
        {
            Account? account = await _context.Accounts
                                        .Where(account =>
                                        account.Email == logInViewModel.Email &&
                                        account.Password == logInViewModel.Password)
                                        .FirstOrDefaultAsync();
            if (account == null)
            {
                ViewData["ErrorMessage"] = "E-mail or password are wrong.";
                return View();
            }
            UserId.SetId(account.Id);

            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, account.Name ?? "null")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);
            return RedirectToAction("Index", "Home");
        }
    }
}
