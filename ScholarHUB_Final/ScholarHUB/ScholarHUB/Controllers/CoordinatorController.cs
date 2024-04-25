using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScholarHUB.Models;
using ScholarHUB.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Plugins;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Security.Claims;

namespace ScholarHUB.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ScholarDbContext _context;
        private readonly ILogger<CoordinatorController> _logger;


        public CoordinatorController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, ScholarDbContext context, ILogger<CoordinatorController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ListAsync()
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
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
            foreach (var article in articles)
            {
                article.Comments = await _context.Comment
                    .Where(c => c.ArticleId == article.ArticleId)
                    .ToListAsync();
            }

            return View(articles);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewData["currentFilter"] = searchString;

            var articles = from a in _context.Article.Include(a => a.Comments)
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(a => a.Title.Contains(searchString) || a.AuthorName.Contains(searchString) || a.FacultyName.Contains(searchString));
            }
            else
            {
                return RedirectToAction("List");
            }

            return View("List", await articles.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSelect(int articleId, Selected selected)
        {
            // Find the article with the given ID
            var article = await _context.Article.FindAsync(articleId);

            if (article == null)
            {
                return NotFound();
            }

            // Update the selected property of the article
            article.Select = selected;
            article.PublishedDate = DateTime.Now;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Add logging statements
            _logger.LogInformation($"Updated article with ID {articleId} to status {selected}");

            return RedirectToAction("List");
        }

        public IActionResult CreateComment()
        {
            string authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.AuthorId = authorId;

            var article = _context.Article.FirstOrDefault();
            if (article != null)
            {
                ViewBag.ArticleId = article.ArticleId;
            }
            else
            {
                ViewBag.ArticleId = null;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int id, [Bind("CommentId, Content, DatePosted, AuthorId")] ScholarHUB.Models.Comment comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Set DatePosted
                    comment.DatePosted = DateTime.Now;

                    // Set AuthorId based on the current user's identity
                    comment.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    // Set ArticleId from the route data
                    comment.ArticleId = id;

                    // Add comment to context
                    _context.Add(comment);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("List");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to add comment. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction("List");
        }
    }
}
