namespace DataAccessLayer.Dto.Account
{
    public class ConsultantProfileUpdateDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public List<CertificationDto>? Certifications { get; set; }
    }
    public class CertificationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Issuer { get; set; }
        public string? Description { get; set; }
        public DateTime? DateIssued { get; set; }
    }
}
