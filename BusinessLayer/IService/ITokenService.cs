using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}
