using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TaskLog
{
    public int TaskLogId { get; set; }

    public int TaskAssignId { get; set; }

    public string? Des { get; set; }

    public int Status { get; set; }

    public virtual TaskAssign TaskAssign { get; set; } = null!;
}
