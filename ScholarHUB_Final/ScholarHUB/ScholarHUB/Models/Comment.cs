using System;
using System.Collections.Generic;

namespace ScholarHUB.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? DatePosted { get; set; }

    public string? AuthorId { get; set; }

    public int? ArticleId { get; set; }

    public virtual Article? Article { get; set; }

    public virtual UserProfile? Author { get; set; }
}
