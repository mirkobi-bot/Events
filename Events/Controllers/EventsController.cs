using Events.Models;
using Events.Models.Entities;
using Events.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Events.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var allUsers = await _eventService.GetAllEventsAsync();

            return Ok(allUsers);
        }
        [HttpGet("{eventId}")]

        public async Task<IActionResult> GetEventById(int eventId)
        {
            var ev = await _eventService.GetEventByIdAsync(eventId);
            if (ev is null) return NotFound();

            return Ok(ev);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDto dto)
        {
            var created = await _eventService.CreateEventAsync(dto);
            return CreatedAtAction(nameof(GetEventById), new { eventId = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            var updated = await _eventService.UpdateEventAsync(dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var deleted = await _eventService.DeleteEventAsync(eventId);
            if (!deleted)
                return NotFound();

            return NoContent(); // 204: success, no body
        }


    }
}
