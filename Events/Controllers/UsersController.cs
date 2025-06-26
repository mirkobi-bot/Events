using Events.Data;
using Events.Models;
using Events.Models.Entities;
using Events.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]

        [HttpGet("user-events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetUserEvents()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var events = await _userService.GetEventsByUser(userId);
            return Ok(events);
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
        public async Task<IActionResult> Login([FromBody] LoginDto dto, [FromServices] JwtService jwt)
        {
            var result = await _userService.LoginAsync(dto.Username, dto.Password, jwt);
            if (result is null)
                return Unauthorized("Invalid username or password.");
            return Ok(result);
        }
    }
    }
