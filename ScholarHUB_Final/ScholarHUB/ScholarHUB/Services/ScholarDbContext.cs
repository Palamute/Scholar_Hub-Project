using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScholarHUB.Models;
using System.Reflection.Emit;
using System.Security.Principal;

namespace ScholarHUB.Services
{
    public class ScholarDbContext: IdentityDbContext<UserProfile>
    {
        public ScholarDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var admin = new IdentityRole("Admin");
            admin.NormalizedName = "Admin";

            var user = new IdentityRole("Student");
            user.NormalizedName = "Student";

            var manager = new IdentityRole("Manager");
            manager.NormalizedName = "Manager";

            var coordinator = new IdentityRole("Coordinator");
            coordinator.NormalizedName = "Coordinator";

            var guest = new IdentityRole("Guest");
            guest.NormalizedName = "Guest";

            builder.Entity<IdentityRole>().HasData(admin, user, manager, coordinator, guest);

           // builder.Entity<UserProfile>().HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleName);

            // Add Faculty
            var informationTechnologyFaculty = new Faculty { FacultyId = 1, FacultyName = "Information Technology" };
            var graphicFaculty = new Faculty { FacultyId = 2, FacultyName = "Graphic Design" };
            var marketingFaculty = new Faculty { FacultyId = 3, FacultyName = "Marketing" };
            var lawFaculty = new Faculty { FacultyId = 4, FacultyName = "Law" };
            var businessFaculty = new Faculty { FacultyId = 5, FacultyName = "Business" };
            builder.Entity<Faculty>().HasData(informationTechnologyFaculty, graphicFaculty, marketingFaculty, lawFaculty, businessFaculty);

            //Add Admin User
            var adminUser = new UserProfile
            {
                Id = "165481d3-58ea-4af4-98b7-6b77fe9608d7",
                FirstName = "Admin",
                LastName = "User",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                RoleId = admin.Id,
                RoleName = admin.Name,
            };
            adminUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(adminUser, "Scholar123.");

            builder.Entity<UserProfile>().HasData(adminUser);

            //Add All Roles to AdminUser
            var adminRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = admin.Id,
                    UserId = adminUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRole);

            // Add Guest IT User
            var guestItUser = new UserProfile
            {
                Id = "6a17c08d-06dc-40ba-a528-00de67f0fdb3",
                FirstName = "Guest",
                LastName = "IT",
                UserName = "guestit@gmail.com",
                Email = "guestit@gmail.com",
                NormalizedEmail = "guestit@gmail.com".ToUpper(),
                NormalizedUserName = "guestit@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = informationTechnologyFaculty.FacultyName,
                RoleId = guest.Id,
                RoleName = guest.Name
            };
            guestItUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(guestItUser, "Guest123.");

            builder.Entity<UserProfile>().HasData(guestItUser);

            //Add Role to Guest IT User
            var guestItRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = guest.Id,
                    UserId = guestItUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(guestItRole);

            //Add Guest Marketing User
            var guestMarketingUser = new UserProfile
            {
                Id = "2f350328-65fd-40ec-b890-ce4af066927f",
                FirstName = "Guest",
                LastName = "Marketing",
                UserName = "guestmarketing@gmail.com",
                Email = "guestmarketing@gmail.com",
                NormalizedEmail = "guestmarketing@gmail.com".ToUpper(),
                NormalizedUserName = "guestmarketing@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = marketingFaculty.FacultyName,
                RoleId = guest.Id,
                RoleName = guest.Name
            };
            guestMarketingUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(guestMarketingUser, "Guest123.");

            builder.Entity<UserProfile>().HasData(guestMarketingUser);

            //Add Role to GuestMarketingUser
            var guestMarketingRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = guest.Id,
                    UserId = guestMarketingUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(guestMarketingRole);

            //Add Guest GraphicDesign User
            var guestGraphicUser = new UserProfile
            {
                Id = "9e90f00f-c69f-4a4d-a5bc-8aac1422f199",
                FirstName = "Guest",
                LastName = "Graphic",
                UserName = "guestgraphic@gmail.com",
                Email = "guestgraphic@gmail.com",
                NormalizedEmail = "guestgraphic@gmail.com".ToUpper(),
                NormalizedUserName = "guestgraphic@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = graphicFaculty.FacultyName,
                RoleId = guest.Id,
                RoleName = guest.Name
            };
            guestGraphicUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(guestGraphicUser, "Guest123.");

            builder.Entity<UserProfile>().HasData(guestGraphicUser);

            //Add Role to GuestGraphicDesignUser
            var guestGraphicRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = guest.Id,
                    UserId = guestGraphicUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(guestGraphicRole);

            //Add Guest Law User
            var guestLawUser = new UserProfile
            {
                Id = "3cd74346-8add-4e66-8b17-69bc4a2e0d16",
                FirstName = "Guest",
                LastName = "Law",
                UserName = "guestlaw@gmail.com",
                Email = "guestlaw@gmail.com",
                NormalizedEmail = "guestlaw@gmail.com".ToUpper(),
                NormalizedUserName = "guestlaw@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = lawFaculty.FacultyName,
                RoleId = guest.Id,
                RoleName = guest.Name
            };
            guestLawUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(guestLawUser, "Guest123.");

            builder.Entity<UserProfile>().HasData(guestLawUser);

            //Add Role to GuestLawDesignUser
            var guestLawRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = guest.Id,
                    UserId = guestLawUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(guestLawRole);

            //Add Guest Business User
            var guestBusinessUser = new UserProfile
            {
                Id = "fab5ec6b-e9ef-4b3b-88bd-5083613ce9bd",
                FirstName = "Guest",
                LastName = "Business",
                UserName = "guestbusiness@gmail.com",
                Email = "guestbusiness@gmail.com",
                NormalizedEmail = "guestbusiness@gmail.com".ToUpper(),
                NormalizedUserName = "guestbusiness@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = businessFaculty.FacultyName,
                RoleId = guest.Id,
                RoleName = guest.Name
            };
            guestBusinessUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(guestBusinessUser, "Guest123.");

            builder.Entity<UserProfile>().HasData(guestBusinessUser);

            //Add Role to GuestLawDesignUser
            var guestBusinessRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = guest.Id,
                    UserId = guestBusinessUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(guestBusinessRole);

            //Add Manager User
            var managerUser = new UserProfile
            {
                Id = "caa72819-738e-4c1b-a03a-81687c2eb101",
                FirstName = "Manager",
                LastName = "User",
                UserName = "manager@gmail.com",
                Email = "manager@gmail.com",
                NormalizedEmail = "manager@gmail.com".ToUpper(),
                NormalizedUserName = "manager@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = informationTechnologyFaculty.FacultyName,
                RoleId = manager.Id,
                RoleName = manager.Name
            };
            managerUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(managerUser, "Manager123.");

            builder.Entity<UserProfile>().HasData(managerUser);

            //Add Role to Manager User
            var managerRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = manager.Id,
                    UserId = managerUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(managerRole);

            //Add IT Coordinator User
            var coordinatorItUser = new UserProfile
            {
                Id = "ab45742a-b3e6-422a-a3c3-16fb834b83b6",
                FirstName = "Coordinator",
                LastName = "IT",
                UserName = "coordinatorit@gmail.com",
                Email = "coordinatorit@gmail.com",
                NormalizedEmail = "coordinatorit@gmail.com".ToUpper(),
                NormalizedUserName = "coordinatorit@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = informationTechnologyFaculty.FacultyName,
                RoleId = coordinator.Id,
                RoleName = coordinator.Name
            };
            coordinatorItUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(coordinatorItUser, "Coordinator123.");

            builder.Entity<UserProfile>().HasData(coordinatorItUser);

            //Add Role to It Coordinator User
            var coordinatorItRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = coordinator.Id,
                    UserId = coordinatorItUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(coordinatorItRole);

            //Add Marketing Coordinator User
            var coordinatorMarketingUser = new UserProfile
            {
                Id = "8097b232-4528-420d-9b9f-bd1b003f7510",
                FirstName = "Coordinator",
                LastName = "Marketing",
                UserName = "coordinatormarketing@gmail.com",
                Email = "coordinatormarketing@gmail.com",
                NormalizedEmail = "coordinatormarketing@gmail.com".ToUpper(),
                NormalizedUserName = "coordinatormarketing@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = marketingFaculty.FacultyName,
                RoleId = coordinator.Id,
                RoleName = coordinator.Name
            };
            coordinatorMarketingUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(coordinatorMarketingUser, "Coordinator123.");

            builder.Entity<UserProfile>().HasData(coordinatorMarketingUser);

            //Add Role to Coordinator Marketing User
            var coordinatorMarketingRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = coordinator.Id,
                    UserId = coordinatorMarketingUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(coordinatorMarketingRole);

            //Add Graphic Design Coordinator User
            var coordinatorGraphicUser = new UserProfile
            {
                Id = "efc7aa07-ee29-49c2-b61b-2dbda36768d4",
                FirstName = "Coordinator",
                LastName = "Graphic",
                UserName = "coordinatorgraphic@gmail.com",
                Email = "coordinatorgraphic@gmail.com",
                NormalizedEmail = "coordinatorgraphic@gmail.com".ToUpper(),
                NormalizedUserName = "coordinatorgraphic@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = graphicFaculty.FacultyName,
                RoleId = coordinator.Id,
                RoleName = coordinator.Name
            };
            coordinatorGraphicUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(coordinatorGraphicUser, "Coordinator123.");

            builder.Entity<UserProfile>().HasData(coordinatorGraphicUser);

            //Add Role to Coordinator Graphic Design User
            var coordinatorGraphicRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = coordinator.Id,
                    UserId = coordinatorGraphicUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(coordinatorGraphicRole);

            //Add Law Coordinator User
            var coordinatorLawUser = new UserProfile
            {
                Id = "ccf5d783-983a-48dc-9c9a-2f5978fcccd7",
                FirstName = "Coordinator",
                LastName = "Law",
                UserName = "coordinatorlaw@gmail.com",
                Email = "coordinatorlaw@gmail.com",
                NormalizedEmail = "coordinatorlaw@gmail.com".ToUpper(),
                NormalizedUserName = "coordinatorlaw@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = lawFaculty.FacultyName,
                RoleId = coordinator.Id,
                RoleName = coordinator.Name
            };
            coordinatorLawUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(coordinatorLawUser, "Coordinator123.");

            builder.Entity<UserProfile>().HasData(coordinatorLawUser);

            //Add Role to Coordinator Law User
            var coordinatorLawRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = coordinator.Id,
                    UserId = coordinatorLawUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(coordinatorLawRole);

            //Add Business Coordinator User
            var coordinatorBusinessUser = new UserProfile
            {
                Id = "2fe187a1-f52c-4c43-9c51-68ded879dd37",
                FirstName = "Coordinator",
                LastName = "Business",
                UserName = "coordinatorbusiness@gmail.com",
                Email = "coordinatorbusiness@gmail.com",
                NormalizedEmail = "coordinatorbusiness@gmail.com".ToUpper(),
                NormalizedUserName = "coordinatorbusiness@gmail.com".ToUpper(),
                AcademicYear = "2024-2025",
                PhoneNumber = null,
                FacultyId = null,
                EmailConfirmed = true,
                FacultyName = businessFaculty.FacultyName,
                RoleId = coordinator.Id,
                RoleName = coordinator.Name
            };
            coordinatorBusinessUser.PasswordHash = new PasswordHasher<UserProfile>()
                .HashPassword(coordinatorBusinessUser, "Coordinator123.");

            builder.Entity<UserProfile>().HasData(coordinatorBusinessUser);

            //Add Role to Coordinator Business User
            var coordinatorBusinessRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = coordinator.Id,
                    UserId = coordinatorBusinessUser.Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(coordinatorBusinessRole);
        }
        public DbSet<ScholarHUB.Models.Article> Article { get; set; } = default!;
        public DbSet<ScholarHUB.Models.Comment> Comment { get; set; } = default!;
        public DbSet<ScholarHUB.Models.Faculty> Faculty { get; set; } = default!;
        public DbSet<ScholarHUB.Models.UserProfile> UserProfile { get; set; } = default!;
    }
}
