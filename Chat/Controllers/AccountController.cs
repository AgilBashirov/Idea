using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Data;
using Chat.Models;
using Chat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VmRegister model)
        {
            if (ModelState.IsValid)
            {
                var customUser = new CustomUser()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(customUser, model.Password);
                await _userManager.AddToRoleAsync(customUser, "Customer");
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(customUser, false);
                    return RedirectToAction("index", "home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }



        //Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VmLogin model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login");
                }
            }
            return View(model);
        }

        //Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");

        }
    }
}
