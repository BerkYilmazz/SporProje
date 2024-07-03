using System;
using System.Collections.Generic;

namespace SporProje.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? CommentText { get; set; }

    public int? TopicId { get; set; }

    public int? UserId { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public virtual Topic? Topic { get; set; }

    public virtual User? User { get; set; }
}
