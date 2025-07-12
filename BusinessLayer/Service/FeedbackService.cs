using AutoMapper;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Feedback;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICourseEnrollmentRepository _courseEnrollmentRepository;
        private readonly IMapper _mapper;


        public FeedbackService(IFeedbackRepository feedbackRepository, ICourseEnrollmentRepository courseEnrollmentRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _courseEnrollmentRepository = courseEnrollmentRepository;
            _mapper = mapper;
        }
        public async Task<bool> IsCourseCompletedAsync(string userId, int courseId)
        {
            var enrollment = await _courseEnrollmentRepository.GetEnrollmentByUserIdAndCourseIdAsync(userId, courseId);
            return enrollment != null && enrollment.IsCompleted;
        }
        public async Task<FeedbackViewDto> CreateFeedbackAsync(FeedbackDto feedbackDto, string userId, int courseId)
        {
            var feedback = new Feedback
            {
                UserId = userId,
                CourseId = courseId,
                Rating = feedbackDto.Rating,
                ReviewText = feedbackDto.ReviewText,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            var createdFeedback = await _feedbackRepository.CreateAsync(feedback);
            return _mapper.Map<FeedbackViewDto>(createdFeedback);
        }

        public async Task<bool> RestoreAndUpdateFeedbackAsync(int feedbackId, FeedbackDto feedbackDto)
        {
            var existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (existingFeedback == null) return false;
            existingFeedback.Rating = feedbackDto.Rating;
            existingFeedback.ReviewText = feedbackDto.ReviewText;
            existingFeedback.IsActive = true;  
            existingFeedback.UpdatedAt = DateTime.Now; 
            return await _feedbackRepository.UpdateAsync(existingFeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null) return false;
            feedback.IsActive = false; 
            return await _feedbackRepository.UpdateAsync(feedback);
        }

        public async Task<(double AverageRating, int TotalFeedbacks)> GetAverageRatingAsync(int courseId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByCourseIdAsync(courseId);

            if (feedbacks == null || !feedbacks.Any())
            {
                return (5, 0);
            }

            var averageRating = feedbacks.Average(f => f.Rating);
            var TotalFeedbacks = feedbacks.Count();

            return (averageRating, TotalFeedbacks);
        }

        public async Task<FeedbackViewDto> GetFeedbackByCourseAndUserAsync(int courseId, string userId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByCourseAndUserAsync(courseId, userId);
            return feedback != null ? _mapper.Map<FeedbackViewDto>(feedback) : null;
        }

        public async Task<IEnumerable<FeedbackViewDto>> GetFeedbacksByCourseIdAsync(int courseId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<FeedbackViewDto>>(feedbacks);
        }

        public async Task<bool> UpdateFeedbackAsync(int feedbackId, FeedbackDto feedbackDto)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null) return false;
            feedback.Rating = feedbackDto.Rating;
            feedback.ReviewText = feedbackDto.ReviewText;
            feedback.UpdatedAt = DateTime.Now;
            return await _feedbackRepository.UpdateAsync(feedback);
        }

        public async Task<FeedbackViewDto> GetFeedbackByIdAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            return feedback != null ? _mapper.Map<FeedbackViewDto>(feedback) : null;
        }

    }
}
