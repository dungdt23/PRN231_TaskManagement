using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Member
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Middlename { get; set; }

    public int Age { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<TaskAssign> TaskAssigns { get; set; } = new List<TaskAssign>();

    public virtual ICollection<TaskComment> TaskCommentReplies { get; set; } = new List<TaskComment>();

    public virtual ICollection<TaskComment> TaskCommentUsers { get; set; } = new List<TaskComment>();

    public virtual ICollection<TeamLeader> TeamLeaders { get; set; } = new List<TeamLeader>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
}
