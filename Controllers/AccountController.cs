using AutoShop.Domain.Entities;
using AutoShop.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModels model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email); // шукаєм чи є такій логін в базі
            if (user != null) // якщо юзер не дорівнює налл це означає що в базі є такий користувач і наш емаіл не буде універсальний
                ModelState.AddModelError("Email", "Такий логін вже зареєстровано"); // додаємо валідацію

            if (ModelState.IsValid)
            {
                user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First().Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModels model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,
                        model.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (model.ReturnUrl != null)
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }








    }
}
