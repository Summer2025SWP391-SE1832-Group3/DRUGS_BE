namespace DataAccessLayer.Dto.Consultation
{
    public class AvailableSlotDto
    {
        public int SlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? ConsultantId { get; set; }
        public string? Status { get; set; } // Available, Booked, Pending, Rejected
    }
} 