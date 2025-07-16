using System;

namespace DataAccessLayer.Dto.Account
{
    public class CertificateViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime DateIssued { get; set; }
    }
} 