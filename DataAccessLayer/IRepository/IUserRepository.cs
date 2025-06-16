using DataAccessLayer.Dto.Account;
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
        public  Task<IdentityResult> RegisterAsyn(RegisterDto model,string currentUserId,string role);
        public Task<IdentityResult> CreateAsync(CreateAccountDto model, string role);
        public  Task<ApplicationUser?> GetUserByUserName(string username);
        public Task<bool> CheckPassword(ApplicationUser user,string password);

        // --- ADMIN MANAGEMENT METHODS ---
        Task<IdentityResult> AdminUpdateUserAsync(string userId, RegisterDto updateDto, string? newRole = null);
        Task<IdentityResult> AdminDeleteUserAsync(string userId);
        Task<List<ApplicationUser>> AdminSearchUsersAsync(string? email, string? username, string? role);
    }
}
