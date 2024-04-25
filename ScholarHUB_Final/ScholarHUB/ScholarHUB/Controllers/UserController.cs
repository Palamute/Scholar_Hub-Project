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
    public class UserController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ScholarDbContext _context;
        private readonly ILogger<CoordinatorController> _logger;


        public UserController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, ScholarDbContext context, ILogger<CoordinatorController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }
        [Authorize(Roles = "Student, Coordinator, Manager")]
        public async Task<IActionResult> Index()
        {
            // Lấy người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            if (User.IsInRole("Manager"))
            {
                var articles = await _context.Article
                    .Include(a => a.Faculty)
                    .Where(a => a.Select == Selected.Approved)
                    .OrderByDescending(a => a.PublishedDate)
                    .ToListAsync();

                return View(articles);
            }
            else
            {
                var articles = await _context.Article
                    .Include(a => a.Faculty)
                    .Where(a => currentUser.FacultyName == a.FacultyName && a.Select == Selected.Approved)
                    .OrderByDescending(a => a.PublishedDate)
                    .ToListAsync();

                return View(articles);
            }
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyArticles()
        {
            // Lấy người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            // Truy vấn cơ sở dữ liệu để lấy các bài báo của người dùng
            var articles = await _context.Article
                .Where(a => a.AuthorId == currentUser.Id)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
            foreach (var article in articles)
            {
                article.Comments = await _context.Comment
                    .Where(c => c.ArticleId == article.ArticleId)
                    .ToListAsync();
            }
            // Trả về chế độ xem hiển thị các bài báo
            return View(articles);
        }
    }
}