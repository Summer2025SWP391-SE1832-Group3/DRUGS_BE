using System;

namespace DataAccessLayer.Dto.Account
{
    public class ConsultantWorkingHourDto
    {
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
} 