using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScholarHUB.Services;
using ScholarHUB.Models;
using Microsoft.AspNetCore.Authorization;

namespace ScholarHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ScholarDbContext _context;

        public FacultyController(UserManager<UserProfile> userManager, ScholarDbContext context, SignInManager<UserProfile> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListFaculty()
        {
            var faculties = await _context.Faculty.ToListAsync();
            return View(faculties);
        }

        public async Task<IActionResult> FacultyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculty.FirstOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        public IActionResult CreateFaculty()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaculty([Bind("FacultyId, FacultyName")] Faculty faculty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(faculty);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListFaculty));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(faculty);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var faculty = await _context.Faculty.FindAsync(id);

            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("FacultyId", "FacultyName")] Faculty faculty)
        {
            _context.Update(faculty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListFaculty));
        }

        private bool FacultyExists(int? id)
        {
            return _context.Faculty.Any(e => e.FacultyId == id);
        }


        // GET: UserController/Delete/5
        public async Task<IActionResult> DeleteFaculty(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculty.FirstOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: UserController/Delete/5
        [HttpPost, ActionName("DeleteFaculty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFacultyConfirmed(int? id, string authorId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculty.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }

            // Lấy tất cả các dữ liệu liên quan
            var users = await _context.Users.Where(u => u.FacultyId == id).ToListAsync();
            var articles = await _context.Article.Where(a => a.AuthorId == authorId).ToListAsync();
            var coordinators = await _context.Users.Where(c => c.FacultyId == id).ToListAsync();

            foreach (var user in users)
            {
                // Xóa user
                _context.Users.Remove(user);
            }

            foreach (var article in articles)
            {
                // Xóa article
                _context.Article.Remove(article);
            }

            foreach (var coordinator in coordinators)
            {
                // Xóa tất cả các comment của coordinator
                var comments = await _context.Comment.Where(com => com.AuthorId == coordinator.Id).ToListAsync();
                _context.Comment.RemoveRange(comments);
                // Xóa coordinator
                _context.Users.Remove(coordinator);
            }

            // Xóa faculty
            _context.Faculty.Remove(faculty);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng đến action ListFaculty sau khi xóa
            return RedirectToAction(nameof(ListFaculty));
        }
    }
}
