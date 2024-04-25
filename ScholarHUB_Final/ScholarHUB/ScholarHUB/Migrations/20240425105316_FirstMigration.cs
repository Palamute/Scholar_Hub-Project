using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScholarHUB.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<int>(type: "int", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId");
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<int>(type: "int", nullable: true),
                    Select = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Article_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Article_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ArticleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId");
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3bc43976-b36a-47c6-a7d3-02c3c8a0e7ca", null, "Manager", "Manager" },
                    { "449064ce-ee65-4d0d-be2d-61b53c3bbfc0", null, "Student", "Student" },
                    { "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", null, "Guest", "Guest" },
                    { "a55e2e87-e683-4ec2-af7d-4cab35264499", null, "Admin", "Admin" },
                    { "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", null, "Coordinator", "Coordinator" }
                });

            migrationBuilder.InsertData(
                table: "Faculty",
                columns: new[] { "FacultyId", "FacultyName" },
                values: new object[,]
                {
                    { 1, "Information Technology" },
                    { 2, "Graphic Design" },
                    { 3, "Marketing" },
                    { 4, "Law" },
                    { 5, "Business" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AcademicYear", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FacultyId", "FacultyName", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleId", "RoleName", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "165481d3-58ea-4af4-98b7-6b77fe9608d7", "2024-2025", 0, "1dd9b018-da1d-457c-a899-22c4e697657c", "admin@gmail.com", true, null, null, "Admin", "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEEIRi1QH8RvfUrEfdX0i4lLz9oqTz2xL50m7DIr3fAo+/V4O8x1Hb6wcbd5juUy6Ow==", null, false, "a55e2e87-e683-4ec2-af7d-4cab35264499", "Admin", "a4a5ffe1-3e4c-4720-8353-280f4870d90b", false, "admin@gmail.com" },
                    { "2f350328-65fd-40ec-b890-ce4af066927f", "2024-2025", 0, "882ac072-864c-46be-bb88-5ed60e2f2873", "guestmarketing@gmail.com", true, null, "Marketing", "Guest", "Marketing", false, null, "GUESTMARKETING@GMAIL.COM", "GUESTMARKETING@GMAIL.COM", "AQAAAAIAAYagAAAAEJcJSSlQA3IFZKz+ABsb76cU3aLxDIMJ7WXSg02KhGgxW4uIU7VfUnPSVroFq5rtAQ==", null, false, "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "Guest", "c0921b49-0cc4-4b6d-8e08-2b7707784a80", false, "guestmarketing@gmail.com" },
                    { "2fe187a1-f52c-4c43-9c51-68ded879dd37", "2024-2025", 0, "44fe629e-4d46-48ba-83cf-312b08ed4d41", "coordinatorbusiness@gmail.com", true, null, "Business", "Coordinator", "Business", false, null, "COORDINATORBUSINESS@GMAIL.COM", "COORDINATORBUSINESS@GMAIL.COM", "AQAAAAIAAYagAAAAEDftHRdWKjIWyWgf0FPo3WJ6MCucogJ6ALv60UV70QZlLRj0FZvV2lH7M6tTh86X8Q==", null, false, "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "Coordinator", "4748e191-a728-4968-9b3e-bdbac3e9fd96", false, "coordinatorbusiness@gmail.com" },
                    { "3cd74346-8add-4e66-8b17-69bc4a2e0d16", "2024-2025", 0, "166fbddf-eaa4-4f64-9790-b762fd8960b4", "guestlaw@gmail.com", true, null, "Law", "Guest", "Law", false, null, "GUESTLAW@GMAIL.COM", "GUESTLAW@GMAIL.COM", "AQAAAAIAAYagAAAAEPz5UK9M38KrJxnZv3bAZRExG24NUIfkU38FashG4AAgYrsAqTnyIxuphLiQnFXTKw==", null, false, "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "Guest", "f204da46-df95-4b65-9e3f-f69ba5be939c", false, "guestlaw@gmail.com" },
                    { "6a17c08d-06dc-40ba-a528-00de67f0fdb3", "2024-2025", 0, "c7652033-7d3c-4a52-8f1b-3a1c8ed4d8d0", "guestit@gmail.com", true, null, "Information Technology", "Guest", "IT", false, null, "GUESTIT@GMAIL.COM", "GUESTIT@GMAIL.COM", "AQAAAAIAAYagAAAAEJ6/GqsEdPiiR5I4EXRYnFPU2V0K+hcuD+FHVuoHOQBc0ooDYDoqlcJTimb123LQWQ==", null, false, "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "Guest", "d95c047f-4220-44c6-a46f-00719b9e0530", false, "guestit@gmail.com" },
                    { "8097b232-4528-420d-9b9f-bd1b003f7510", "2024-2025", 0, "7a4c481b-6391-4d81-9607-0c055c1252b4", "coordinatormarketing@gmail.com", true, null, "Marketing", "Coordinator", "Marketing", false, null, "COORDINATORMARKETING@GMAIL.COM", "COORDINATORMARKETING@GMAIL.COM", "AQAAAAIAAYagAAAAEEJSJ35OyiPvn5ysAvE6wWfbjaGPyIxZZl4n+H8l9oFejtQVLHbXwLvl4U8UtuWKzQ==", null, false, "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "Coordinator", "9139af86-577d-42c2-b3a3-b8b2dbe3d843", false, "coordinatormarketing@gmail.com" },
                    { "9e90f00f-c69f-4a4d-a5bc-8aac1422f199", "2024-2025", 0, "ecaf9056-42f7-45fb-af7a-ea5eb28392ee", "guestgraphic@gmail.com", true, null, "Graphic Design", "Guest", "Graphic", false, null, "GUESTGRAPHIC@GMAIL.COM", "GUESTGRAPHIC@GMAIL.COM", "AQAAAAIAAYagAAAAEInNrCyCRsZJYA9ae61pnUXtTY+CahQXXGMg/sde/7Azs+jMVRwmFUW2VE7jAsCyyg==", null, false, "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "Guest", "f9b191fb-daa4-41e9-8221-070b8e922f49", false, "guestgraphic@gmail.com" },
                    { "ab45742a-b3e6-422a-a3c3-16fb834b83b6", "2024-2025", 0, "85bc3155-2968-4237-9526-8c72310c14c0", "coordinatorit@gmail.com", true, null, "Information Technology", "Coordinator", "IT", false, null, "COORDINATORIT@GMAIL.COM", "COORDINATORIT@GMAIL.COM", "AQAAAAIAAYagAAAAELt1CUcIezc6VjzpFSkW6XHnEnDnsfoxXfcjZoAKFSgNgSK6mu4rJ45U19EZmS5vcg==", null, false, "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "Coordinator", "c92ebb8d-b201-429c-a77d-8a0436dd105b", false, "coordinatorit@gmail.com" },
                    { "caa72819-738e-4c1b-a03a-81687c2eb101", "2024-2025", 0, "20984d7d-169d-4806-9585-8f41d144ac0a", "manager@gmail.com", true, null, "Information Technology", "Manager", "User", false, null, "MANAGER@GMAIL.COM", "MANAGER@GMAIL.COM", "AQAAAAIAAYagAAAAEKLoNJvHMcOtDHmZvbEPHnaRkfnOymVbEjmhSt2UOUS/vzFMUI7L876+zeWSb+zWrA==", null, false, "3bc43976-b36a-47c6-a7d3-02c3c8a0e7ca", "Manager", "4e4c844e-0e88-4c6b-85e5-fab43d146cea", false, "manager@gmail.com" },
                    { "ccf5d783-983a-48dc-9c9a-2f5978fcccd7", "2024-2025", 0, "34a29d4a-5228-4fd8-98be-66e80748f492", "coordinatorlaw@gmail.com", true, null, "Law", "Coordinator", "Law", false, null, "COORDINATORLAW@GMAIL.COM", "COORDINATORLAW@GMAIL.COM", "AQAAAAIAAYagAAAAEK5KCMe0k82XoHYtPCNfdIqkLXmbPno86jhHDQ3qFCa+CIDIkyhT1+NzCx2sn/9ebg==", null, false, "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "Coordinator", "fc611e53-abaa-4431-9133-b146c61fd0ec", false, "coordinatorlaw@gmail.com" },
                    { "efc7aa07-ee29-49c2-b61b-2dbda36768d4", "2024-2025", 0, "1e95f44a-fc1b-4af2-a20e-5b95561d7eef", "coordinatorgraphic@gmail.com", true, null, "Graphic Design", "Coordinator", "Graphic", false, null, "COORDINATORGRAPHIC@GMAIL.COM", "COORDINATORGRAPHIC@GMAIL.COM", "AQAAAAIAAYagAAAAEIqNPCl4C1AVAVx9t7KDGBUMIHx9HDxBPmmvqnIkbtpPSDCFb5FeifKLJjsHLzatZg==", null, false, "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "Coordinator", "d6c49b6e-fed2-4710-bdb0-cdfe1c520538", false, "coordinatorgraphic@gmail.com" },
                    { "fab5ec6b-e9ef-4b3b-88bd-5083613ce9bd", "2024-2025", 0, "80e343f9-98a0-41b0-829c-f9370878795a", "guestbusiness@gmail.com", true, null, "Business", "Guest", "Business", false, null, "GUESTBUSINESS@GMAIL.COM", "GUESTBUSINESS@GMAIL.COM", "AQAAAAIAAYagAAAAEHXHeBDZUCdKU24pseUsLxk+CorWLw1o/pHL0imJimrRTmCTjIJjKQfD4M6XZQio0g==", null, false, "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "Guest", "a1fa6cba-9219-4af9-b400-2c49216c94d6", false, "guestbusiness@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a55e2e87-e683-4ec2-af7d-4cab35264499", "165481d3-58ea-4af4-98b7-6b77fe9608d7" },
                    { "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "2f350328-65fd-40ec-b890-ce4af066927f" },
                    { "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "2fe187a1-f52c-4c43-9c51-68ded879dd37" },
                    { "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "3cd74346-8add-4e66-8b17-69bc4a2e0d16" },
                    { "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "6a17c08d-06dc-40ba-a528-00de67f0fdb3" },
                    { "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "8097b232-4528-420d-9b9f-bd1b003f7510" },
                    { "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "9e90f00f-c69f-4a4d-a5bc-8aac1422f199" },
                    { "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "ab45742a-b3e6-422a-a3c3-16fb834b83b6" },
                    { "3bc43976-b36a-47c6-a7d3-02c3c8a0e7ca", "caa72819-738e-4c1b-a03a-81687c2eb101" },
                    { "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "ccf5d783-983a-48dc-9c9a-2f5978fcccd7" },
                    { "aef5cdcb-b7e6-4c41-8c83-4eaad52a245a", "efc7aa07-ee29-49c2-b61b-2dbda36768d4" },
                    { "a304a3dc-4303-4c9d-988b-bb2bcd64ea80", "fab5ec6b-e9ef-4b3b-88bd-5083613ce9bd" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_AuthorId",
                table: "Article",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_FacultyId",
                table: "Article",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FacultyId",
                table: "AspNetUsers",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ArticleId",
                table: "Comment",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Faculty");
        }
    }
}
