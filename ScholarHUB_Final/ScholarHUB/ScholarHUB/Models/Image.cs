using System;
using System.Collections.Generic;

namespace ScholarHUB.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string FilePath { get; set; } = null!;

    public int? ArticleId { get; set; }

    public virtual Article? Article { get; set; }
}
