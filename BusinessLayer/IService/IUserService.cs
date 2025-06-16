using DataAccessLayer.Dto;
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
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto, string? role, string defaultRole);
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<IdentityResult> AdminDeleteUserAsync(string userId);
        Task<bool> IsAdmin(string userId);
        Task<IEnumerable<ApplicationUser>> AdminSearchUsersAsync(string? email, string? username, string? role);
        Task<IdentityResult> UpdateUserProfileAsync(string userId, UserProfileUpdateDto dto);
    } 
} 
