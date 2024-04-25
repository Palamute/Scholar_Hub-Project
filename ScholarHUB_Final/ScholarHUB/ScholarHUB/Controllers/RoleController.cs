using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScholarHUB.Models;
using ScholarHUB.Services;

namespace ScholarHUB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly ScholarDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<UserProfile> userManager, ScholarDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {
            // Lấy danh sách các IdentityRole từ context
            var identityRoles = await _context.Roles
                .OrderBy(u => u.Name)
                .ToListAsync();

            // Khởi tạo danh sách các Role
            var roles = new List<Role>();

            // Chuyển đổi danh sách các IdentityRole thành danh sách các Role
            foreach (var identityRole in identityRoles)
            {
                var role = new Role
                {
                    Id = identityRole.Id,
                    Name = identityRole.Name
                };
                roles.Add(role);
            }

            // Truyền danh sách các Role vào view
            return View(roles);
        }

        public async Task<IActionResult> Details(string? id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            var customRole = new Role
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(customRole);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(List));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(role);
        }

        private bool RoleExists(string? id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }


        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            var customRole = new ScholarHUB.Models.Role
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(customRole);
        }

        // POST: UserController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
        public async Task<IActionResult> ListOfUsers()
        {
            var users = await _context.Users
                .OrderBy(u => u.RoleName)
                .ToListAsync();
            var userEmailAndRoles = new List<(string Email, string Roles)>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var roles = string.Join(", ", userRoles); // Kết hợp các vai trò thành một chuỗi
                userEmailAndRoles.Add((user.Email, roles));
            }

            return View(userEmailAndRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string email)
        {
            // Kiểm tra xem email có tồn tại trong danh sách người dùng không
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Lấy danh sách email và vai trò từ cơ sở dữ liệu
            var userEmails = await _context.Users.Select(u => u.Email).ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            // Lấy vai trò hiện tại của người dùng
            var currentRole = await _userManager.GetRolesAsync(user);

            ViewBag.UserEmails = userEmails;
            ViewBag.Roles = roles;
            ViewBag.SelectedEmail = email;
            ViewBag.CurrentRole = currentRole.FirstOrDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string email, string roleId)
        {
            // Kiểm tra email và roleId không rỗng
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleId))
            {
                return BadRequest();
            }

            // Tìm người dùng bằng email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Lấy vai trò mới từ roleId
            var newRole = await _roleManager.FindByIdAsync(roleId);
            if (newRole == null)
            {
                return NotFound("Role not found");
            }

            // Lấy danh sách các vai trò hiện tại của người dùng
            var userRoles = await _userManager.GetRolesAsync(user);

            // Nếu người dùng không có vai trò mới trong danh sách vai trò hiện tại
            if (!userRoles.Contains(newRole.Name))
            {
                // Thêm vai trò mới cho người dùng
                var resultAddNewRole = await _userManager.AddToRoleAsync(user, newRole.Name);
                if (!resultAddNewRole.Succeeded)
                {
                    return View("Error");
                }

                // Cập nhật RoleId và RoleName của người dùng trong bảng AspNetUsers
                user.RoleId = newRole.Id;
                user.RoleName = newRole.Name;
                await _userManager.UpdateAsync(user);

                // Xóa các vai trò cũ của người dùng
                var resultRemoveOldRoles = await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());
                if (!resultRemoveOldRoles.Succeeded)
                {
                    return View("Error");
                }

                // Xử lý khi thêm và xóa vai trò thành công
                return RedirectToAction(nameof(ListOfUsers));
            }
            else
            {
                // Người dùng đã có vai trò mới
                return RedirectToAction(nameof(ListOfUsers));
            }

        }

    }
}