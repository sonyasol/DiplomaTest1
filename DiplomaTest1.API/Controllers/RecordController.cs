using DiplomaTest1.Core.DTOs.Record;
using DiplomaTest1.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaTest1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _recordService.GetRecordByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("vaccine/{vaccineId}")]
        public async Task<IActionResult> GetByVaccineId(int vaccineId)
        {
            var result = await _recordService.GetRecordByVaccineIdAsync(vaccineId);
            return Ok(result);
        }

        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRecordDto dto)
        {
            try
            {
                var created = await _recordService.AddRecordAsync(dto);
                return CreatedAtAction(nameof(GetByUserId),
                    new {userId = created.Id}, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidCastException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "Doctor, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _recordService.DeleteRecordAsync(id);
            if (!result)
                return NotFound($"Запись с  Id = {id} не найдена");
            return NoContent();
        }
    } 
}
