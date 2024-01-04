using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Requests
{
    public class UserTeamRequest
    {
        public int TeamId { get; set; }

        public int UserId { get; set; }
        public string? Role { get; set; }
        public string? Des { get; set; }
    }
}
