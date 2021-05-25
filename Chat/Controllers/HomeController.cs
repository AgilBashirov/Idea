using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chat.Models;
using Chat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Chat.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin, Moderator, User, Customer")]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);

            List<CustomUser> customUsers = _context.CustomUsers.Where(u=>u.Id != userId).ToList();
            return View(customUsers);
        }

        [Authorize(Roles = "Admin, Moderator, User, Customer")]
        public IActionResult Chat(string friendId)
        {
            if (friendId == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User);

            ViewBag.Friend = _context.CustomUsers.Find(friendId);

            List<Message> messages = _context.Messages.Include(u => u.Sender)
                                                      .Where(m => (m.SenderId == friendId && m.ReceiverId == userId) ||
                                                                  (m.SenderId == userId && m.ReceiverId == friendId))
                                                      .OrderBy(o => o.Date)
                                                      .ToList();

            foreach (var item in messages)
            {
                item.IsRead = true;
                _context.Update(item);
            }
            _context.SaveChanges();

            return View(messages);
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
