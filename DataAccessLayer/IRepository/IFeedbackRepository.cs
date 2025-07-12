using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{

    public interface IFeedbackRepository
    {
        Task<Feedback> CreateAsync(Feedback feedback);
        Task<Feedback> GetFeedbackByCourseAndUserAsync(int courseId, string userId);
        Task<bool> UpdateAsync(Feedback feedback);
        Task<bool> DeleteAsync(int feedbackId);
        Task<IEnumerable<Feedback>> GetFeedbacksByCourseIdAsync(int courseId);
        Task<Feedback> GetFeedbackByIdAsync(int feedbackId);

    }
}
