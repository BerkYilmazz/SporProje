using System;
using System.Collections.Generic;

namespace SporProje.Models;

public partial class Topic
{
    public int TopicId { get; set; }

    public string? Title { get; set; }

    public string? ContentText { get; set; }

    public int? UserId { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? User { get; set; }
}
