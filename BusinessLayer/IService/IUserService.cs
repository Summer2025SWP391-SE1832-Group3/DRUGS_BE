using DataAccessLayer.Dto;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model; 

namespace BusinessLayer.IService
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto res, string currentUserIdm, string role);
        Task<string> LoginAsync(LoginDto login);
        Task<IdentityResult> AdminCreateUserAsync(RegisterDto dto, string role);
        Task<IdentityResult> AdminUpdateUserAsync(string userId, RegisterDto dto, string role);
        Task<IdentityResult> AdminDeleteUserAsync(string userId);
        Task<List<ApplicationUser>> AdminSearchUsersAsync(string? email, string? phone, string? role); 
    } 
} 
