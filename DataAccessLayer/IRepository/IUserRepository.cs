using DataAccessLayer.Dto;
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
        public  Task<ApplicationUser> GetUserByEmail(string email);
    }
}
