using Events.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Events.Models;
using Events.Models.Entities;
using Events.Services;

namespace Events.Controllers
{
    //va su localhost::porta/api/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsersAsync();

            return Ok(allUsers);
        }

        [HttpGet("{userId}")]

        public async Task<IActionResult> GetUserById(int userId)
        {
            var u = await _userService.GetUserByIdAsync(userId);
            if (u is null) return NotFound();

            return Ok(u);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            var created = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { userId = created.Id }, created);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UserDto dto)
        {
            if (userId != dto.Id)
                return BadRequest("ID mismatch.");

            var updated = await _userService.UpdateUserAsync(dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var deleted = await _userService.DeleteUserAsync(userId);
            if (!deleted)
                return NotFound();

            return NoContent(); // 204: success, no body
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var userId = await _userService.LoginAsync(dto.Username, dto.Password);
            return Ok(userId); // -1 se fallito, altrimenti l'ID
        }
    }
    }
