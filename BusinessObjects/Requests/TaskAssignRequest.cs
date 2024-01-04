using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Requests
{
    public class TaskAssignRequest
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public string? Des { get; set; }

        public DateTime AssignDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
