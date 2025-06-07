using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface IUserService
    {
         public Task<IdentityResult> RegisterAsync(RegisterDto res);
         public Task<string> LoginAsync(LoginDto login);

    }
}
