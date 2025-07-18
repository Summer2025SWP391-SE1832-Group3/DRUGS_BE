﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.BlogPost
{
    public class CommentUpdateDto
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
