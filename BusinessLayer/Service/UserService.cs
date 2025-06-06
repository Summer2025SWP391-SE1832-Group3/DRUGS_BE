using BusinessLayer.IService;
using DataAccessLayer.Dto;
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

        public UserService(IUserRepository userRepository,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager) {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public Task<string> LoginAsync(LoginDto login)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> RegisterAsync(RegisterDto res)
        {
            var result = _userRepository.CreateUserAsyn(res);
            return result;
        }
    }
}
