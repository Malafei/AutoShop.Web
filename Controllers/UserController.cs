﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoShop.Domain;
using AutoShop.Domain.Entities;
using AutoShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AutoShop.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppEFContext _context;

        public UserController(AppEFContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserCreateViewMode usermodel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == usermodel.Login); // шукаєм чи є такій логін в базі
                if (user == null) // якщо юзер дорівнює налл це означає що в базі нема такого логіна і наш логін буде універсальний
                {
                    // добавляем користувача в бд
                    _context.Users.Add(new User { Login = usermodel.Login, Password = usermodel.Password, KeyWord = usermodel.KeyWord});
                    await _context.SaveChangesAsync(); //зберігаєм зміни

                    //await Authenticate(usermodel.Login); // аутентифікація

                    return RedirectToAction("LoginAut", "User"); // переходим на сторінку входу
                }
                else
                ModelState.AddModelError("Login", "Такий логін вже зареєстровано"); // додаємо валідацію
            }
            return View(usermodel);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginAut(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAut(UserSiginVievModel usermodel)
        {
            if (!ModelState.IsValid) // перевіряєм на валідність
                return View(usermodel);    //якщо невалідне повертаєм невалідну вюшку

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == usermodel.Login && u.Password == usermodel.Password);
            // створюєм нового юзера та шукаєм в базі такогож
            if (user != null) //якщо знайшли змінна юзер не буде дорівнювати налл і можна працювати далі
            {
                await Authenticate(usermodel.Login); // автентифікуємось

                return Redirect(usermodel.ReturnUrl); //переходим по посилані куди ми хотіли спочатку
            }
            ModelState.AddModelError("Password", "Некорректные логин и(или) пароль"); //в модел стат повідомляєм про невалідність
            return View(usermodel); // та повертаєм невалідну вюшку

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearthToEditPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SearthToEditPassword(SearthUserEditVievModel usermodel)
        {
            if (!ModelState.IsValid) // перевіряєм на валідність
                return View(usermodel);    //якщо невалідне повертаєм невалідну вюшку
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == usermodel.Login);
            if (user != null) //якщо знайшли змінна юзер не буде дорівнювати налл і можна працювати далі
            {
                return RedirectToAction("AuditUserToEditPassword", "User", new { id = user.Id });
                //return RedirectToAction("AuditUserToEditPassword", "User", user); // переходим на сторінку входу
            }

            ModelState.AddModelError("Login", "Користувача з таким логіном не знайдено"); //в модел стат повідомляєм про невалідність
            return View(usermodel); // та повертаєм невалідну вюшку
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AuditUserToEditPassword(int id)
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult AuditUserToEditPassword(AuditUserEditVievModel usermodel, int id)
        {
            if (!ModelState.IsValid) // перевіряєм на валідність
                return View(usermodel);    //якщо невалідне повертаєм невалідну вюшку
            User user = _context.Users.Find(id);
            if (user != null)
            {
                if (user.KeyWord == usermodel.Word)
                {
                    return RedirectToAction("EditPassword", "User", new { id = user.Id });
                }
                ModelState.AddModelError("Word", "Невірне слово перевірте та спробуйте знову"); //в модел стат повідомляєм про невалідність
            }
            return View(usermodel); // та повертаєм невалідну вюшку
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditPassword(int id)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult EditPassword(EditPasswordVievModel usermodel, int id)
        {
            if (!ModelState.IsValid) // перевіряєм на валідність
                return View(usermodel);    //якщо невалідне повертаєм невалідну вюшку
            User user = _context.Users.Find(id);
            user.Password = usermodel.NewPassword;

            _context.Entry(user).State = EntityState.Modified; /*Кажем, що такій об'єкт в базі данних вже є і нам потрібно лише редагувати його*/
            _context.SaveChanges(); /*Сохраняємо*/
            return RedirectToAction("Index", "home");
        }









        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(string userName)
        {
            // создаєм один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаєм объект ClaimsIdentity
            ClaimsIdentity clamIdentite = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентифікаціонних кукі
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clamIdentite));
        }

    }
}
