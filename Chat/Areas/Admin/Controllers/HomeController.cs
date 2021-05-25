using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Areas.Admin.ViewModels;
using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chat.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(AppDbContext context, UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Users
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Users()
        {
            VmUser model = new VmUser()
            {
                CustomUsers = _context.CustomUsers.ToList(),
                Roles = _roleManager.Roles.ToList()
            };

            //var roles = _roleManager.Roles.ToList();

            return View(model);
        }

        public IActionResult UpdateUser(string id)
        {
            //user
            CustomUser user = _context.CustomUsers.Find(id);

            //old role
            var userRole = _context.UserRoles.FirstOrDefault(r => r.UserId == id);

            //all roles
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach (var item in _context.Roles.ToList())
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Value = item.Id;
                selectListItem.Text = item.Name;

                roles.Add(selectListItem);
            }

            user.Roles = roles;
            if (userRole != null)
            {
                user.RoleId = userRole.RoleId;
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(CustomUser model)
        {
            if (ModelState.IsValid)
            {
                CustomUser customUser = _context.CustomUsers.Find(model.Id);
                customUser.Name = model.Name;

                var selectedRole = _context.Roles.Find(model.RoleId);
                if (selectedRole == null)
                {
                    return NotFound();
                }

                var oldRole = _context.UserRoles.FirstOrDefault(r => r.UserId == customUser.Id);
                if (oldRole != null)
                {
                    await _userManager.RemoveFromRoleAsync(customUser, _context.Roles.Find(oldRole.RoleId).Name);
                }
                await _userManager.AddToRoleAsync(customUser, selectedRole.Name);
                _context.SaveChanges();
                return RedirectToAction("Users");
            }

            return View(model);
        }


        //Roles
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Roles()
        {
            List<IdentityRole> roles = _context.Roles.ToList();

            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole model)
        {
            var result = await _roleManager.CreateAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("roles", "home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityRole role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(IdentityRole model)
        {
            var result = await _roleManager.UpdateAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("roles", "home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


    }
}
