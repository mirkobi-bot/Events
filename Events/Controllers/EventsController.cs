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
        public async Task<IActionResult> SaveEvent([FromBody] EventDto e)
        {
            var result = await _eventService.SaveEventAsync(e, User);
            if (result == null)
                return BadRequest("Errore nel salvataggio evento");

            return Ok(result);
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
