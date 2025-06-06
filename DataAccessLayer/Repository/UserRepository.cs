using DataAccessLayer.Dto;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
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

        public UserRepository(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
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
                    return IdentityResult.Failed();
                }
                var addrole = await _userManager.AddToRoleAsync(user, "Member");
                if (!addrole.Succeeded)
                {
                    return IdentityResult.Failed();
                }
                    return result;
            }
            catch(Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = "An unexpected error occurred." });
            }
        }

        public Task<ApplicationUser> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
