namespace DataAccessLayer.Dto.Account
{
    public class ConsultantViewDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int TotalConsultations { get; set; }
        public double AverageRating { get; set; }
        public int FeedbackCount { get; set; }
    }
} 