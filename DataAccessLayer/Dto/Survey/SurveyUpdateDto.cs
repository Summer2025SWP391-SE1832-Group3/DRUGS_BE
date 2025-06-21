using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyUpdateDto
    {
        [Required]
        public int SurveyId { get; set; }
        [Required]
        [MaxLength(100)]
        public string SurveyName { get; set; } = null!;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
