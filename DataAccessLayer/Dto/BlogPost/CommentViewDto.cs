using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.BlogPost
{
    public class CommentViewDto
    {
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CommentAt { get; set; }
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }
        [Required]
        public int BlogId { get; set; }
    }
}
