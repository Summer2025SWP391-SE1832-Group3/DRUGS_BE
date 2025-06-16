using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class BlogImage
    {
        public int BlogImageId { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        public Blog Blog { get; set; }
        public int BlogId { get; set; }

    }
}
