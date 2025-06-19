using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string SurveyName { get; set; } = null!; 
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        [MaxLength(50)]
        public SurveyType SurveyType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
