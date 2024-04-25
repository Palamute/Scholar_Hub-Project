 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ScholarHUB.Models;
using ScholarHUB.Services;
using Microsoft.EntityFrameworkCore;
using ScholarHub.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ScholarHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IUserStore<UserProfile> _userStore;
        private readonly IUserEmailStore<UserProfile> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ScholarDbContext _context;

        public AdminController(UserManager<UserProfile> userManager,
            IUserStore<UserProfile> userStore,
            SignInManager<UserProfile> signInManager,
            ILogger<RegisterModel> logger,
            ScholarDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> List()
        {
            var users = await _context.Users
                .OrderBy(u => u.RoleName)
                .ToListAsync();
            return View(users);
        }


        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var academicYear = user.AcademicYear;

            ViewBag.AcademicYear = academicYear;

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Faculty"] = await _context.Faculty.ToListAsync();

            var roles = await _context.Roles.ToListAsync();

            var roleList = roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            ViewData["Roles"] = roleList;

            return View();
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterModel.InputModel model)
        {
            // Retrieve the list of faculties
            ViewData["Faculty"] = await _context.Faculty.ToListAsync();

            ViewData["Roles"] = await _context.Roles.ToListAsync();

            // Set default password for the new user
            model.Password = "Scholarhub123.";
            model.ConfirmPassword = "Scholarhub123.";

            // Create a new UserProfile object from the form data
            var user = new UserProfile
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                FacultyName = model.FacultyName,
                AcademicYear = model.AcademicYear,
                EmailConfirmed = true,
            };

            // Create the user with the UserManager and hash the password
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Add the user to the selected role
                var selectedRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == model.RoleId);
                if (selectedRole != null)
                {
                    await _userManager.AddToRoleAsync(user, selectedRole.Name);

                    // Lưu RoleId và RoleName vào UserProfile và cập nhật cơ sở dữ liệu
                    user.RoleId = selectedRole.Id;
                    user.RoleName = selectedRole.Name;
                    _context.Update(user); // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                    await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                }
                // Send an email to the user with the account information
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("minhtvagcs210898@fpt.edu.vn", "hyib noen jwia qllk"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("minhtvagcs210898@fpt.edu.vn"),
                    Subject = "Your Scholarhub Account Information",
                    Body = $"Your account has been created. Your USERNAME is {model.Email} and your default PASSWORD is 'Scholarhub123.'"
                };

                mailMessage.To.Add(model.Email);

                await smtpClient.SendMailAsync(mailMessage);

                // Redirect to the List action
                return RedirectToAction(nameof(List));
            }

            // Return the view with the RegisterModel.InputModel object
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAcademicYear(UserProfile model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(model.Id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Update the academic year for the user
                    user.AcademicYear = model.AcademicYear;

                    // Save changes to the database
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Details", new { id = model.Id });
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle the exception (e.g., concurrency conflict)
                    ModelState.AddModelError("", "Concurrency error occurred.");
                }
            }

            // If the model state is invalid, redisplay the form with validation errors
            return View("Details", model);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Tìm và xóa bình luận của người dùng
            var userComments = _context.Comment.Where(c => c.AuthorId == id);
            _context.Comment.RemoveRange(userComments);

            // Loại bỏ người dùng
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
