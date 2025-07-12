using DataAccessLayer.Dto.Feedback;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface IFeedbackService
    {
        Task<FeedbackViewDto> CreateFeedbackAsync(FeedbackDto feedbackDto, string userId, int courseId);

        Task<bool> UpdateFeedbackAsync(int feedbackId, FeedbackDto feedbackDto);

        Task<bool> DeleteFeedbackAsync(int feedbackId);

        Task<IEnumerable<FeedbackViewDto>> GetFeedbacksByCourseIdAsync(int courseId);
        Task<FeedbackViewDto> GetFeedbackByCourseAndUserAsync(int courseId, string userId);
        Task<(double AverageRating, int TotalFeedbacks)> GetAverageRatingAsync(int courseId);
        Task<bool> IsCourseCompletedAsync(string userId, int courseId);
        Task<bool> RestoreAndUpdateFeedbackAsync(int feedbackId, FeedbackDto feedbackDto);
        Task<FeedbackViewDto> GetFeedbackByIdAsync(int feedbackId);
    }
}
