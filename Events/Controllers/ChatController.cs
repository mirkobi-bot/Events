using Events.Models;
using Events.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [ApiController]

    [Route("api/events/{eventId}/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET api/events/{eventId}/chat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int eventId)
        {
            var messages = await _chatService.GetMessagesByEventAsync(eventId);
            return Ok(messages);
        }

        // POST api/events/{eventId}/chat
        [HttpPost]
        public async Task<ActionResult<MessageDto>> PostMessage(int eventId, CreateMessageDto dto)
        {
            if (eventId != dto.EventId)
                return BadRequest("Event ID mismatch.");

            var message = await _chatService.AddMessageAsync(dto);
            return CreatedAtAction(nameof(GetMessages), new { eventId = eventId }, message);
        }
    }
    }
