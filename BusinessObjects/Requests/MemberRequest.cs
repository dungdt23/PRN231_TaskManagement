using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Requests
{
    public class MemberRequest
    {

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string? Middlename { get; set; }

        public int Age { get; set; }

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsDelete { get; set; }
    }
}
