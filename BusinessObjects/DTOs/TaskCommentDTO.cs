using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class TaskCommentDTO
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public int? ReplyId { get; set; }

        public string CommentContent { get; set; } = null!;

        public DateTime Date { get; set; }

        public bool IsEdit { get; set; }

        public bool IsDelete { get; set; }
    }
}
