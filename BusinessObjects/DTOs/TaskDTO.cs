using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class TaskDTO
    {
        public int TaskId { get; set; }

        public string Taskname { get; set; } = null!;

        public int CategoryId { get; set; }

        public string? Des { get; set; }

        public int TeamId { get; set; }

        public bool IsDelete { get; set; }
        public int? Status { get; set; }

        public virtual CategoryDTO Category { get; set; } = null!;
        public virtual TeamDTO Team { get; set; } = null!;
    }
}
