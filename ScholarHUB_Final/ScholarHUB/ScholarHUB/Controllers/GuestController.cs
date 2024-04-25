using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScholarHUB.Controllers;
using ScholarHUB.Models;
using ScholarHUB.Services;
using System.Threading.Tasks;

namespace ScholarHub.Controllers
{
    [Authorize(Roles = "Guest")]
    public class GuestController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ScholarDbContext _context;
        private readonly ILogger<CoordinatorController> _logger;


        public GuestController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, ScholarDbContext context, ILogger<CoordinatorController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }
            // Get all articles from the specified faculties
            var articles = await _context.Article
                .Include(a => a.Faculty)
                .Where(a => currentUser.FacultyName == a.FacultyName)
                .ToListAsync();

            return View(articles);
        }
    }
}