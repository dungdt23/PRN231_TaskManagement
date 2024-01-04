using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Rolename { get; set; } = null!;

    public string? Des { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
