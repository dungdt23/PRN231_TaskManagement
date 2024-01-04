using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TaskAssign
{
    public int TaskAssignId { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string? Des { get; set; }

    public bool IsDelete { get; set; }

    public DateTime AssignDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool? Status { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();

    public virtual Member User { get; set; } = null!;
}
