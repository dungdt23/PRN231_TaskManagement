using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class UserRoleDTO
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }

        public string? Des { get; set; }

        public bool IsDelete { get; set; }
    }
}
