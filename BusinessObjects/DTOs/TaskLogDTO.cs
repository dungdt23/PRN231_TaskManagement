using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class TaskLogDTO
    {
        public int TaskAssignId { get; set; }

        public string? Des { get; set; }

        public int Status { get; set; }
    }
}
