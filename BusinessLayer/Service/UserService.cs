using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public UserService(
            IUserRepository userRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<string> LoginAsync(LoginDto login)
        {
            var user = await _userRepository.GetUserByUserName(login.UserName);
            if (user == null)
            {
                return null;
            }
            var validPassword = await _userRepository.CheckPassword(user, login.Password);
            if (!validPassword)
            {
                return null;
            }
            return await _tokenService.CreateToken(user);
        }


        public async Task<IdentityResult> RegisterAsync(RegisterDto res, string currentUserId, string role = "Member")
        {
            var existingEmail = await _userManager.FindByEmailAsync(res.Email);
            if (existingEmail != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailAlreadyExists",
                    Description = "Email is already taken"
                });
            }

            var existingUserName = await _userManager.FindByNameAsync(res.UserName);
            if (existingUserName != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNameAlreadyExists",
                    Description = "UserName is already taken"
                });
            }

            var result = await _userRepository.CreateUserAsyn(res, currentUserId, role);
            return result;
        }

        public async Task<IdentityResult> AdminCreateUserAsync(RegisterDto dto, string role)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded && !string.IsNullOrEmpty(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return result;
        }

        public async Task<IdentityResult> AdminUpdateUserAsync(string userId, RegisterDto dto, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded && !string.IsNullOrEmpty(role))
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, role);
            }
            return result;
        }

        public async Task<IdentityResult> AdminDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<ApplicationUser>> AdminSearchUsersAsync(string? username, string? email, string? role)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(email))
                users = users.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrEmpty(username))
                users = users.Where(u => u.UserName.Contains(username));

            var userList = users.ToList();

            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = new List<ApplicationUser>();
                foreach (var user in userList)
                {
                    if (await _userManager.IsInRoleAsync(user, role))
                        usersInRole.Add(user);
                }
                return usersInRole;
            }

            return userList;
        }
    }
}
