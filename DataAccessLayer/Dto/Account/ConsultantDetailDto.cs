using System.Collections.Generic;
using DataAccessLayer.Model;

namespace DataAccessLayer.Dto.Account
{
    public class ConsultantDetailDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public IEnumerable<ConsultantWorkingHour> WorkingHours { get; set; }
        public IEnumerable<Certificate> Certificates { get; set; }
    }
} 