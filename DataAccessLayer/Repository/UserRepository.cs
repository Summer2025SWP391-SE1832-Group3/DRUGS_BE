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

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateUserAsyn(RegisterDto model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
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
                    return IdentityResult.Failed(result.Errors.ToArray());
                }
                var addrole = await _userManager.AddToRoleAsync(user, "Member");
                if (!addrole.Succeeded)
                {
                    return IdentityResult.Failed(addrole.Errors.ToArray());
                }
                    return result;  
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        public async Task<ApplicationUser> GetUserByUserName(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==username);
        }

        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
