﻿using DataAccessLayer.Data;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class CourseReportRepository: ICourseReportRepository
    {
        private readonly ApplicationDBContext _context;

        public CourseReportRepository(ApplicationDBContext context) {
            _context=context; 
        }

        public async Task<CourseReportDto> GetCourseReportAsync(int courseId)
        {
            var enrollments = await _context.CourseEnrollments
                                            .Where(e => e.CourseId == courseId)
                                            .ToListAsync();

            var completedCount = enrollments.Count(e => e.IsCompleted);
            var report = new CourseReportDto
            {
                CourseId = courseId,
                TotalEnrollments = enrollments.Count,
                CompletedCount = completedCount,
                PendingCount = enrollments.Count - completedCount
            };

            return report;
        }

        public async Task<LessonProgressReportDto> GetLessonProgressReportAsync(int courseId)
        {
            var enrollments = await _context.CourseEnrollments
            .Where(e => e.CourseId == courseId)
            .Include(e => e.User)
            .ToListAsync();

            var lessonProgressReports = new List<LessonProgressDetailDto>();

            foreach (var enrollment in enrollments)
            {
                var progress = await _context.LessonProgresses
                    .Where(p => p.CourseEnrollmentId == enrollment.Id)
                    .ToListAsync();

                lessonProgressReports.Add(new LessonProgressDetailDto
                {
                    UserId = enrollment.UserId,
                    TotalLessons = progress.Count(),
                    CompletedLessons = progress.Count(p => p.IsCompleted)
                });
            }

            return new LessonProgressReportDto
            {
                CourseId = courseId,
                Reports = lessonProgressReports
            };
        }
    }
}
