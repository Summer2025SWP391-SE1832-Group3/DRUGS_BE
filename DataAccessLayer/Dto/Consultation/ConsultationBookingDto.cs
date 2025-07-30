namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationBookingCreateDto
    {
        public string ConsultantId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class ConsultationBookingDto
    {
        public int Id { get; set; }
        public string? ConsultantId { get; set; }
        public string? ConsultantName { get; set; }
        public string? MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
    }
}