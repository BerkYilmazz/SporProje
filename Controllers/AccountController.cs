using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SporProje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TrendyModa.Controllers
{

    public class AccountController : Controller
    {
        private readonly SporDbContext context;
        public AccountController()
        {
            context = new SporDbContext();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] User user)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(user, user.Password);
                user.Password = hashedPassword;
                user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromForm, Bind("Email", "Password")] User dataUser)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == dataUser.Email);

            if (user == null || !VerifyHashedPassword(user.Password, dataUser.Password))
            {
                return View(dataUser);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Password", user.Password),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim("UserId", user.UserId.ToString()),
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        private bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }


        [Authorize]
        public async Task<IActionResult> LogoutIndex()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpGet]
        public IActionResult AccountEdit()
        {
            int claimId = Convert.ToInt32(User.FindFirst(x => x.Type == "UserId").Value);
            var u = context.Users.FirstOrDefault(y => y.UserId == claimId);
            return View(u);
        }

        [HttpPost]
        public IActionResult AccountEditConfirmed([FromForm] User user)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(user, user.Password);
                user.Password = hashedPassword;
                context.Users.Update(user);
                context.SaveChanges();
                return RedirectToAction("LogoutIndex", "Account");
            }
            return RedirectToAction("Index", "Home");
        }

    }

}