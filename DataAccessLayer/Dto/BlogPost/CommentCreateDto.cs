using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.BlogPost
{
    public class CommentCreateDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int BlogId { get; set; }
    }
}
