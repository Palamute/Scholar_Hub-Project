using DocumentFormat.OpenXml.Wordprocessing;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace ScholarHUB.Models;

public partial class Article
{
    public int ArticleId { get; set; }
    public string Title { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? Email { get; set; }
    public string? FacultyName { get; set; }
    public int? FacultyId { get; set; }
    public Selected Select { get; set; }
    public virtual UserProfile? Author { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual Faculty? Faculty { get; set; }}
public enum Selected
{
    Public,
    Pending, // Đang chờ
    Approved, // Đã được chấp nhận
    Rejected // Bị từ chối
}