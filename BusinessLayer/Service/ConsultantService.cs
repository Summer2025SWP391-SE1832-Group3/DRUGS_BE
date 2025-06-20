using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Service
{
    public class ConsultantService : IConsultantService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ConsultantService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ConsultantViewDto>> GetAllConsultantsAsync()
        {
            var users = _userManager.Users.ToList();
            var consultants = new List<ConsultantViewDto>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Consultant"))
                {
                    consultants.Add(new ConsultantViewDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Gender = user.Gender
                    });
                }
            }
            return consultants;
        }

        public async Task<ConsultantDetailDto> GetConsultantDetailAsync(string consultantId)
        {
            var user = await _userManager.FindByIdAsync(consultantId);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "Consultant")))
                return null;
            return new ConsultantDetailDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Description = null 
            };
        }
    }
} 