using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _consultantService;
        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ConsultantViewDto>>> GetAllConsultants()
        {
            var consultants = await _consultantService.GetAllConsultantsAsync();
            return Ok(consultants);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ConsultantDetailDto>> GetConsultantDetail(string id)
        {
            var consultant = await _consultantService.GetConsultantDetailAsync(id);
            if (consultant == null)
                return NotFound();
            return Ok(consultant);
        }
    }
} 