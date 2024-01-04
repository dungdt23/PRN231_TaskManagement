using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Requests
{
    public class TeamRequest
    {
        public string Teamname { get; set; } = null!;

        public string? Des { get; set; }

        public int ProjectId { get; set; }
    }
}
