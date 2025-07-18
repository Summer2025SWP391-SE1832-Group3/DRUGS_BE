﻿using DataAccessLayer.Dto.Account;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IUserRepository
    {
        public  Task<IdentityResult> RegisterAsync(RegisterDto model,string currentUserId,string role);
        public Task<IdentityResult> CreateAsync(CreateAccountDto model, string role);
        public  Task<ApplicationUser?> GetUserByUserName(string username);
        public Task<bool> CheckPassword(ApplicationUser user,string password);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetByIdAsync(string userId);

        // --- ADMIN MANAGEMENT METHODS ---
        Task<IdentityResult> AdminUpdateUserAsync(string userId, RegisterDto updateDto, string? newRole = null);
        Task<IdentityResult> AdminDeleteUserAsync(string userId);
        Task<List<ApplicationUser>> AdminSearchUsersAsync(string? email, string? username, string? role);

        // --- CONSULTANT ADVANCED METHODS ---
        Task<List<ApplicationUser>> GetConsultantsByStatusAsync(string status);
        Task<List<ApplicationUser>> GetTopConsultantsByPerformanceAsync(int topN);
        Task<List<ApplicationUser>> GetTopConsultantsByRatingAsync(int topN);
        Task UpdateConsultantStatusAsync(string consultantId, string status);
        Task UpdateConsultantPerformanceAsync(string consultantId, int totalConsultations, double averageRating, int feedbackCount);

        //Task<ApplicationUser?> GetByIdAsync(string userId);
    }
}
