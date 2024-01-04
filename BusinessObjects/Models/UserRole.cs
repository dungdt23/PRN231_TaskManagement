using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public string? Des { get; set; }

    public bool IsDelete { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Member User { get; set; } = null!;
}
