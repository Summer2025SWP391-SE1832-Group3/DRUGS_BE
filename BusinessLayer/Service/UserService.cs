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

        public UserService(IUserRepository userRepository,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager,ITokenService tokenService) {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService= tokenService;
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

                var result =await _userRepository.CreateUserAsyn(res,currentUserId,role);
                return result;
        }
    }
}
