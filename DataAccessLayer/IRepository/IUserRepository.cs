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
        public  Task<IdentityResult> CreateUserAsyn(RegisterDto model);
        public  Task<ApplicationUser> GetUserByUserName(string username);
        public Task<bool> CheckPassword(ApplicationUser user,string password);

    }
}
