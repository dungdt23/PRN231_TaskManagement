using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class TaskAssignDTO
    {
        public int TaskAssignId { get; set; }

        public int TaskId { get; set; }

        public int UserId { get; set; }

        public string? Des { get; set; }

        public bool IsDelete { get; set; }
        public virtual TaskDTO Task { get; set; } = null!;
        public virtual MemberDTO User { get; set; } = null!;

        public DateTime AssignDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public bool? Status { get; set; }

    }
}
