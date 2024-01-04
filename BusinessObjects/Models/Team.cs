using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public string Teamname { get; set; } = null!;

    public string? Des { get; set; }

    public int ProjectId { get; set; }

    public bool IsDelete { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<TeamLeader> TeamLeaders { get; set; } = new List<TeamLeader>();

    public virtual ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
}
