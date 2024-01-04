using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class UserTeam
{
    public int UserTeamId { get; set; }

    public int TeamId { get; set; }

    public int UserId { get; set; }

    public string? Des { get; set; }

    public bool IsDelete { get; set; }

    public string? Role { get; set; }

    public bool? IsLeader { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual Member User { get; set; } = null!;
}
