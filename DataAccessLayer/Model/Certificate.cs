using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class Certificate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string IssuingOrganization { get; set; }

        public DateTime DateIssued { get; set; }

        // Foreign Key to ApplicationUser
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
} 