using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Categoryname { get; set; } = null!;

    public string? Des { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
