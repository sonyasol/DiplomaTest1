using DiplomaTest1.Core.DTOs.Vaccine;
using DiplomaTest1.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaTest1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineService _vaccineService;
        public VaccineController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vaccineService.GetAllVaccinesAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _vaccineService.GetVaccineByIdAsync(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVaccine(CreateVaccineDto dto)
        {
            try
            {
                var result = await _vaccineService.CreateVaccineAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
