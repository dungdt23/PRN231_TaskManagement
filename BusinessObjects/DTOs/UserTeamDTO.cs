using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class UserTeamDTO
    {
        public int UserTeamId { get; set; }

        public int TeamId { get; set; }

        public int UserId { get; set; }

        public string? Des { get; set; }

        public bool IsDelete { get; set; }
        public string? Role { get; set; }
        public bool? IsLeader { get; set; }

        public virtual TeamDTO Team { get; set; }
        public virtual MemberDTO User { get; set; }
    }
}
