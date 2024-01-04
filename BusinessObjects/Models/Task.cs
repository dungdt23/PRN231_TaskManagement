using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string Taskname { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? Des { get; set; }

    public int TeamId { get; set; }

    public bool IsDelete { get; set; }

    public int? Status { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<TaskAssign> TaskAssigns { get; set; } = new List<TaskAssign>();

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual Team Team { get; set; } = null!;
}
