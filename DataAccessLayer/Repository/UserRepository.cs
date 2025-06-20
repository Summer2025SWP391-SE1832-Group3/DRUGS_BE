using DataAccessLayer.Dto.Account;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UserRepository : IUserRepository
    
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsyn(RegisterDto model,string currentUserId, string role)
        {
            // Log toàn bộ dữ liệu đầu vào
            Console.WriteLine($"[DEBUG] RegisterDto: UserName={model.UserName}, Email={model.Email}, FullName={model.FullName}, Password={model.Password}, DateOfBirth={model.DateOfBirth}, Gender={model.Gender}, PhoneNumber={model.PhoneNumber}");
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                Console.WriteLine("[DEBUG] Một số trường bắt buộc bị thiếu!");
                return IdentityResult.Failed(new IdentityError { Description = "Some fields are empty." });
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                CreatedAt = DateTime.Now,
                PhoneNumber = model.PhoneNumber,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) {
                    Console.WriteLine("[DEBUG] IdentityResult khi tạo user KHÔNG thành công:");
                    foreach (var err in result.Errors)
                    {
                        Console.WriteLine($"[DEBUG] IdentityError: {err.Code} - {err.Description}");
                    }
                    return IdentityResult.Failed(result.Errors.ToArray());
                }
                var createdUser = await _userManager.FindByNameAsync(model.UserName);
                if (createdUser == null)
                {
                    Console.WriteLine($"[DEBUG] User creation failed: user is null after creation. Username: {model.UserName}");
                    return IdentityResult.Failed(new IdentityError { Description = $"User creation failed (user is null after creation, username: {model.UserName})." });
                }
                string roleToAssign = null;
                if(currentUserId!=null && await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(currentUserId), "Admin")){
                    roleToAssign = role;
                } else {
                    roleToAssign = "Member";
                }
                if (!string.IsNullOrEmpty(roleToAssign))
                {
                    var roleManager = (RoleManager<IdentityRole>)_userManager.GetType().GetProperty("RoleManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_userManager);
                    bool roleExists = false;
                    if (roleManager != null)
                    {
                        roleExists = await roleManager.RoleExistsAsync(roleToAssign);
                    }
                    else
                    {
                        roleExists = true;
                    }
                    if (!roleExists)
                    {
                        Console.WriteLine($"[DEBUG] Role '{roleToAssign}' does not exist in database. Không gán role cho user!");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(createdUser, roleToAssign);
                        Console.WriteLine($"[DEBUG] User '{model.UserName}' đã được tạo và gán role '{roleToAssign}' thành công.");
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[DEBUG] Exception: {ex.Message}, Username: {model.UserName}, Email: {model.Email}");
                return IdentityResult.Failed(new IdentityError { Description = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        public async Task<ApplicationUser?> GetUserByUserName(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==username);
        }

        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        // --- ADMIN MANAGEMENT METHODS ---
        public async Task<IdentityResult> AdminUpdateUserAsync(string userId, RegisterDto updateDto, string? newRole = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            user.UserName = updateDto.UserName ?? user.UserName;
            user.Email = updateDto.Email ?? user.Email;
            user.FullName = updateDto.FullName ?? user.FullName;
            user.DateOfBirth = updateDto.DateOfBirth != default ? updateDto.DateOfBirth : user.DateOfBirth;
            user.Gender = updateDto.Gender ?? user.Gender;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return result;

            if (!string.IsNullOrEmpty(newRole))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole);
            }
            return result;
        }

        public async Task<IdentityResult> AdminDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<ApplicationUser>> AdminSearchUsersAsync(string? email, string? username, string? role)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.Contains(email));
            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.UserName.Contains(username));
            var users = await query.ToListAsync();

            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = new List<ApplicationUser>();
                foreach (var user in users)
                {
                    if (await _userManager.IsInRoleAsync(user, role))
                        usersInRole.Add(user);
                }
                return usersInRole;
            }
            return users;
        }
    }
}
