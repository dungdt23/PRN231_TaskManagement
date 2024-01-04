using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }

        public string Projectname { get; set; } = null!;

        public string? Des { get; set; }

        public bool IsDelete { get; set; }

        public int Status { get; set; }

    }
}
