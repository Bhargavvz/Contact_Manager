using ContactManager.Data;
using ContactManager.Models;
using ContactManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Dashboard");
            }
            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var dashboardData = new DashboardViewModel
            {
                TotalContacts = await _context.Contacts.CountAsync(c => c.UserId == userId),
                FavoriteContacts = await _context.Contacts.CountAsync(c => c.UserId == userId && c.IsFavorite),
                ContactsAddedThisMonth = await _context.Contacts.CountAsync(c => c.UserId == userId && 
                    c.DateCreated.Month == DateTime.Now.Month && c.DateCreated.Year == DateTime.Now.Year),
                RecentContacts = await _context.Contacts
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.DateCreated)
                    .Take(5)
                    .ToListAsync(),
                FavoriteContactsList = await _context.Contacts
                    .Where(c => c.UserId == userId && c.IsFavorite)
                    .OrderBy(c => c.Name)
                    .Take(5)
                    .ToListAsync()
            };
            
            return View(dashboardData);
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
