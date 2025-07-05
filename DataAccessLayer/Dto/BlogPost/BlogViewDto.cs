using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.BlogPost
{
    public class BlogViewDto
    {
        public int BlogId { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [MaxLength(100)]
        public DateTime PostedAt { get; set; }
        public string? Category { get; set; }
        public string PostedBy { get; set; }
        public BlogStatus Status { get; set; }
        public List<CommentViewDto> Comments { get; set; }

        public List<string> BlogImages { get; set; }
    }
}
