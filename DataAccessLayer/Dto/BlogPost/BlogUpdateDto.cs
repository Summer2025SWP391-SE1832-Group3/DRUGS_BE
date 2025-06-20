using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.BlogPost
{
    public class BlogUpdateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Category { get; set; }
    }
}
