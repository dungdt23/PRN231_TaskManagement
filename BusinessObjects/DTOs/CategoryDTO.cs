using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string Categoryname { get; set; } = null!;

        public string? Des { get; set; }
    }
}
