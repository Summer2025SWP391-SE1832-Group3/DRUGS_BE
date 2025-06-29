using System;

namespace DataAccessLayer.Dto.Account
{
    public class ConsultantWorkingHourDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
} 