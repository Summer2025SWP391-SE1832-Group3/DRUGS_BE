using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CommentAt { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
