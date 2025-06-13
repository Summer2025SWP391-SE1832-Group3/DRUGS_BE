using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public enum BlogStatus
    {
        Pending=0,
        Approved=1,
        Rejected=2
    }
    public class Blog
    {
        public int BlogId { get; set; }
        [Required,MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        [Required, MaxLength(20)]
        public BlogStatus Status { get; set; } = BlogStatus.Pending;
        [Required]
        public string PostedById { get; set; }
        public ApplicationUser PostedBy { get; set; }
        public string? ApprovedById { get; set; }
        public ApplicationUser? ApprovedBy { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<BlogImage> BlogImages { get; set; }
    }
}
