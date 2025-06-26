using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Account
{
    public class CertificateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string IssuingOrganization { get; set; }

        public DateTime DateIssued { get; set; }
    }
} 