using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Dto.Account;

namespace BusinessLayer.IService
{
    public interface IConsultantService
    {
        Task<IEnumerable<ConsultantViewDto>> GetAllConsultantsAsync();
        Task<ConsultantDetailDto> GetConsultantDetailAsync(string consultantId);
    }
} 