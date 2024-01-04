using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string Projectname { get; set; } = null!;

    public string? Des { get; set; }

    public bool IsDelete { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
