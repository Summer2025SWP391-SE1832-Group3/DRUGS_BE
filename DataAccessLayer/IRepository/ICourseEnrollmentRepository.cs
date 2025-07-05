﻿using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ICourseEnrollmentRepository
    {
        Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId);
        Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseIdAsync(int courseId);

        Task<bool> UpdateStatus(string userId, int courseId);
    }
}
