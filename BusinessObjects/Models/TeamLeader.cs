using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TeamLeader
{
    public int TeamLeaderId { get; set; }

    public int UserId { get; set; }

    public int TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual Member User { get; set; } = null!;
}
