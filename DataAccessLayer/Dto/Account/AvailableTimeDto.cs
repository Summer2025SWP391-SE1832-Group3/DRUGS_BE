namespace DataAccessLayer.Dto.Account
{
    public class AvailableTimeDto
    {
        public int WorkingHourId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
