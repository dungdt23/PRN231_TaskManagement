using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TaskComment
{
    public int CommentId { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public int? ReplyId { get; set; }

    public string CommentContent { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool IsEdit { get; set; }

    public bool IsDelete { get; set; }

    public virtual Member? Reply { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual Member User { get; set; } = null!;
}
