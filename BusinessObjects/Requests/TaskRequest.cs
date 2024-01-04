using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Requests
{
    public class TaskRequest
    {
        public string Taskname { get; set; } = null!;

        public int CategoryId { get; set; }

        public string? Des { get; set; }

        public int TeamId { get; set; }
    }
}
