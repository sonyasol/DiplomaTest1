using DiplomaTest1.Core.DTOs.User;
using DiplomaTest1.Core.Interfaces;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaTest1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Doctor, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result == null) { return NotFound(); }

            return Ok(result);
        }

        [Authorize(Roles = "Doctor, Admin")]
        [HttpGet("snils/{snils}")]
        public async Task<IActionResult> GetBySnils(string snils)
        {
            var result = await _userService.GetUserBySnilsAsync(snils);
            if (result == null) { return NotFound(); }
            
            return Ok(result);
        }

        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            try
            {
                var result = await _userService.CreateAsync(dto);
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
