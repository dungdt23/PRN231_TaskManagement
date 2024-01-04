using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Requests
{
    public class ProjectRequest
    {
        public string Projectname { get; set; } = null!;

        public string? Des { get; set; }

        public int Status { get; set; }
    }
}
