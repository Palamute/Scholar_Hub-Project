using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aspose.Words.Saving;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScholarHUB.Models;
using ScholarHUB.Services;
using Aspose.Words;
using Aspose.Words.Rendering;
using System.IO.Compression;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace ScholarHUB.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ScholarDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<UserProfile> _userManager;

        public ArticleController(ScholarDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<UserProfile> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userFacultyName = User.Claims
                .FirstOrDefault(c => c.Type == "FacultyName")?.Value;

            if (string.IsNullOrEmpty(userFacultyName))
            {
                var articles = _context.Article
                    .OrderBy(a => a.FacultyName)
                    .OrderByDescending(a => a.PublishedDate)
                    .ToList();
                return View(articles);
            }
            else
            {
                var articles = _context.Article
                    .Where(a => a.FacultyName == userFacultyName)
                    .OrderByDescending(a => a.PublishedDate)
                    .ToList();
                return View(articles);
            }
        }
        [Authorize(Roles = "Manager")]
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
                return RedirectToAction("Index");
            }

            return View("Index", await articles.ToListAsync());
        }


        [Authorize(Roles = "Student")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile docxFile, IFormFile imageFile, string title, bool selected)
        {
            try
            {
                // Check if either the docx file or the title is empty
                if (docxFile == null || docxFile.Length == 0 || string.IsNullOrEmpty(title))
                {
                    return BadRequest("Invalid docx file or title.");
                }

                // Check if the docx file is not a docx format
                string docxFileExtension = Path.GetExtension(docxFile.FileName).ToLower();
                if (docxFileExtension != ".docx")
                {
                    return BadRequest("Invalid docx file format. Only .docx files are allowed.");
                }

                // Check if the image file is provided and not empty
                string imagePath = null;
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Check if the image file is not an image format
                    string imageFileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                    if (imageFileExtension != ".jpg" && imageFileExtension != ".jpeg" && imageFileExtension != ".png" && imageFileExtension != ".gif")
                    {
                        return BadRequest("Invalid image file format. Only .jpg, .jpeg, .png, and .gif files are allowed.");
                    }

                    // Save the image file to the appropriate directory
                    imagePath = await SaveFileAsync(imageFile);
                }

                // Save the docx file to the appropriate directory
                string docxFilePath = await SaveFileAsync(docxFile);

                var currentUser = await _userManager.GetUserAsync(User);

                // Check if the current user exists
                if (currentUser == null)
                {
                    return BadRequest("User not authenticated.");
                }

                // Get UserProfile of the current user
                var userProfile = await _context.UserProfile.FirstOrDefaultAsync(u => u.Id == currentUser.Id);

                // Check if the user profile exists
                if (userProfile == null)
                {
                    return BadRequest("User profile not found.");
                }

                // Set the initial status to pending
                Selected select = Selected.Pending;

                // Create and add the article to the database
                var article = new Article
                {
                    Title = title,
                    FilePath = Path.Combine("uploads", docxFilePath),
                    ImagePath = imagePath != null ? Path.Combine("images", imagePath) : null,
                    CreatedDate = DateTime.Now,
                    AuthorId = userProfile.Id,
                    AuthorName = userProfile.FirstName + " " + userProfile.LastName,
                    Email = userProfile.Email,
                    FacultyName = userProfile.FacultyName,
                    FacultyId = userProfile.FacultyId,
                    Select = select
                };

                _context.Article.Add(article);
                await _context.SaveChangesAsync();

                // Get the coordinator's email
                var coordinatorEmails = await _context.UserProfile
                     .Where(up => up.Role.Name.Contains("Coordinator"))
                     .Select(up => up.Email)
                     .ToListAsync();

                if (coordinatorEmails.Any())
                {
                    // Create the email message
                    var message = new MailMessage
                    {
                        From = new MailAddress("minhtvagcs210898@fpt.edu.vn"),
                        Subject = "New Article Created",
                        Body = $"A new article titled '{title}' has been created by {userProfile.UserName}. Please approve or reject the article."
                    };

                    // Add the recipient's email address
                    foreach (var email in coordinatorEmails)
                    {
                        message.To.Add(email);
                    }

                    // Send the email
                    var client = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("minhtvagcs210898@fpt.edu.vn", "hyib noen jwia qllk")
                    };

                    await client.SendMailAsync(message);
                }

                return RedirectToAction("Index", "User");

            }
            catch (Exception ex)
            {
                // Log the exception
                // You can log it to a file, database, or output window
                Console.WriteLine($"Error in Create method: {ex}");

                // Return an internal server error
                return StatusCode(500, "Internal server error");
            }
        }


        private async Task<string> SaveFileAsync(IFormFile file)
        {
            // Define the uploads directory path
            var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var imagesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            // Ensure the uploads directory exists
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            // Ensure the images directory exists
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            // Generate a unique file name for the uploaded file
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(file.ContentType.StartsWith("image") ? imagesDirectory : uploadsDirectory, fileName);

            // Save the uploaded file to the correct directory
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }


        [Authorize(Roles = "Student, Coordinator, Manager, Guest")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student, Coordinator, Manager, Guest")]
        public async Task<IActionResult> Details(int id, [Bind("ArticleId,Title,FilePath,Status,CreatedDate,PublishedDate,AuthorId,FacultyName,FacultyId")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        [Authorize(Roles = "Student, Coordinator, Manager, Guest")]
        public async Task<IActionResult> Render(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var article = await _context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, article.FilePath);

            if (Path.GetExtension(filePath) == ".docx")
            {
                string pdfFilePath = ConvertDocxToPdf(filePath);

                if (string.IsNullOrEmpty(pdfFilePath))
                {
                    // Xử lý lỗi khi chuyển đổi không thành công
                    return BadRequest("Failed to convert Word to PDF.");
                }

                string pdfFileName = Path.GetFileName(pdfFilePath);
                ViewBag.PdfFilePath = pdfFileName;

                return View(article);
            }
            else
            {
                // Xử lý lỗi khi file không phải là.docx hoặc image
                return BadRequest("Invalid file format. Only.docx files are allowed.");
            }
        }

        private string ConvertDocxToPdf(string docxFilePath)
        {
            try
            {
                // Load the Word document
                Aspose.Words.Document doc = new Aspose.Words.Document(docxFilePath);

                // Initialize the PDF options
                Aspose.Words.Saving.PdfSaveOptions options = new Aspose.Words.Saving.PdfSaveOptions();

                // Set the compliance level to PDF/A-1a
                options.Compliance = Aspose.Words.Saving.PdfCompliance.PdfA1a;

                // Convert the Word document to PDF
                string pdfFilePath = Path.ChangeExtension(docxFilePath, ".pdf");
                doc.Save(pdfFilePath, options);

                return pdfFilePath;
            }
            catch (Exception ex)
            {
                // Handle the error
                Console.WriteLine("Failed to convert DOCX to PDF: " + ex.Message);
                return null;
            }
        }

        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FirstOrDefaultAsync(m => m.ArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article
                                        .Include(a => a.Comments) // Include comments related to the article
                                        .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            // Remove comments associated with the article
            _context.Comment.RemoveRange(article.Comments);

            // Remove the article itself
            _context.Article.Remove(article);

            // Gửi email thông báo cho người đăng bài (nếu bài viết đang ở trạng thái "pending")
            if (article.Select == Selected.Pending)
            {
                var userProfile = await _userManager.FindByEmailAsync(article.Email);
                if (userProfile != null)
                {
                    var emailSubject = "Your Pending Article Has Been Rejected.";
                    var emailMessage = $"Dear {userProfile.FirstName},\n\nYour pending article titled '{article.Title}' has been rejected by the coordinator, if there is any problem please contact us.\n\nBest regards,\nThe Scholarhub Team";

                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("minhtvagcs210898@fpt.edu.vn", "hyib noen jwia qllk"),
                        EnableSsl = true
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("minhtvagcs210898@fpt.edu.vn"),
                        Subject = emailSubject,
                        Body = emailMessage
                    };

                    mailMessage.To.Add(article.Email);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Coordinator");
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ArticleId == id);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult DownloadZip()
        {
            var filePaths = Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "uploads"));

            MemoryStream memoryStream = new MemoryStream();
            using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    zipArchive.CreateEntryFromFile(filePath, fileName);
                }
            }
            memoryStream.Position = 0;

            return File(memoryStream, "application/zip", "FileDownloaded.zip");
        }


    }
}
