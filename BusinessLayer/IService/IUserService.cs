﻿using DataAccessLayer.Dto;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model; 
using BusinessLayer.Dto.Common;

namespace BusinessLayer.IService
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto res, string currentUserIdm, string role);
        Task<IdentityResult> CreateAccountAsync(CreateAccountDto res,string role);
        Task<string> LoginAsync(LoginDto login);
        Task<IdentityResult> AdminDeleteUserAsync(string userId);
        Task<IdentityResult> DeactivateUserAsync(string userId);
        Task<IdentityResult> ActivateUserAsync(string userId);
        Task<bool> IsAdmin(string userId);
        Task<IEnumerable<ApplicationUser>> AdminSearchUsersAsync(string? email, string? username, string? role);

        Task<IdentityResult> UpdateUserProfileAsync(string userId, UserProfileUpdateDto dto);
        Task<List<AccountViewDto>> GetAllNonAdminAccountsAsync(string status);

        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task<IdentityResult> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto);
        Task<IdentityResult> UpdateUserPasswordAsync(string userId, string newPassword);
        Task<IdentityResult> UpdateUserRoleAsync(string userId, string newRole);
        
        // Pagination methods
        Task<PaginatedResult<AccountViewDto>> GetPaginatedUsersAsync(int page, int pageSize, string? searchTerm = null, string? role = null, string? status = null);
        Task<PaginatedResult<ConsultantViewDto>> GetPaginatedConsultantsAsync(int page, int pageSize, string? searchTerm = null);
    } 
} 
