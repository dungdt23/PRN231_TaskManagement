using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class TeamDTO
    {
        public int TeamId { get; set; }

        public string Teamname { get; set; } = null!;

        public string? Des { get; set; }

        public int ProjectId { get; set; }

        public bool IsDelete { get; set; }

        public virtual ProjectDTO Project { get; set; } = null!;


    }
}
