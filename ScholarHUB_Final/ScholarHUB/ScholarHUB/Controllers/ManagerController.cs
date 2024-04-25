using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScholarHUB.Models;
using ScholarHUB.Services;

namespace ScholarHUB.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ScholarDbContext _context;

        public ManagerController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, ScholarDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public ActionResult Index()
        {
            // Lấy số bài báo theo từng faculty như bạn đã làm
            var facultyCounts = _context.Article
                .Where(a => a.Select == Selected.Approved)
                .GroupBy(a => a.FacultyName)
                .Select(g => new { FacultyName = g.Key, Count = g.Count() })
                .ToList();

            // Lấy số bài báo theo từng năm
            var articleCountsByYear = _context.Article
                .Where(a => a.Select == Selected.Approved)
                .GroupBy(a => a.PublishedDate.Year)
                .Select(g => new { Year = g.Key, Count = g.Count() })
                .ToList();

            var facultyCountsByYear = _context.Article
                .Where(a => a.Select == Selected.Approved)
                .GroupBy(a => new { a.PublishedDate.Year, a.FacultyName })
                .Select(g => new { Year = g.Key.Year, FacultyName = g.Key.FacultyName, Count = g.Count() })
                .ToList();

            var data = new
            {
                ArticleCountsByYear = articleCountsByYear,
                FacultyCountsByYear = facultyCountsByYear
            };

            // Truyền cả hai dữ liệu vào ViewBag
            ViewBag.FacultyCounts = JsonConvert.SerializeObject(facultyCounts);
            ViewBag.ChartData = JsonConvert.SerializeObject(data);

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
