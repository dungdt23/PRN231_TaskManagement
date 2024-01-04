using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class RoleDTO
    {
        public string Rolename { get; set; } = null!;

        public string? Des { get; set; }

        public bool IsDelete { get; set; }
    }
}
