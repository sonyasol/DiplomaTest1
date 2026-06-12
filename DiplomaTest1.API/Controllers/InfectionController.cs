using DiplomaTest1.BLL;
using DiplomaTest1.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaTest1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InfectionController : ControllerBase
    {
        private readonly IInfectionService _infectionService;
        public InfectionController(IInfectionService infectionService)
        {
            _infectionService = infectionService;
        }
        [Authorize(Roles = "Doctor, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _infectionService.GetAllAsync();
            return Ok(result);
        }

    }
}
