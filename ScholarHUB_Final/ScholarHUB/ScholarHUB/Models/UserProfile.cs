using Microsoft.AspNetCore.Identity;

namespace ScholarHUB.Models
{
    public class UserProfile : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FacultyName { get; set; }
        public int? FacultyId { get; set; }
        public string? AcademicYear { get; set; }
        public override string? Email { get; set; }
        public override string? PhoneNumber { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual Faculty? Faculty { get; set; }
        public virtual IdentityRole? Role { get; set; }
    }
}
