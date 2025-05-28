using Events.Data;
using Events.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Events.Services
{
    public interface IChatService
    {
        Task<IEnumerable<MessageDto>> GetMessagesByEventAsync(int eventId);
        Task<MessageDto> AddMessageAsync(CreateMessageDto dto);
    }

    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesByEventAsync(int eventId)
        {
            return await _context.Set<MessageDto>()
                .FromSqlInterpolated($"EXEC GetMessagesByEvent @EventId = {eventId}")
                .ToListAsync();
        }

        //public async Task<IEnumerable<MessageDto>> GetMessagesByEventAsync(int eventId)
        //{
        //    return await _context.Messages
        //        .Where(m => m.EventId == eventId)
        //        .OrderBy(m => m.SentAt)
        //        .Select(m => new MessageDto
        //        {
        //            Id = m.Id,
        //            Content = m.Content,
        //            SentAt = m.SentAt,
        //            UserId = m.UserId,
        //            EventId = m.EventId,
        //            Username = m.User.Username
        //        })
        //        .ToListAsync();
        //}

        public async Task<MessageDto> AddMessageAsync(CreateMessageDto dto)
        {
            var message = new Message
            {
                Content = dto.Content,
                EventId = dto.EventId,
                UserId = dto.UserId,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(dto.UserId);

            return new MessageDto
            {
                Id = message.Id,
                Content = message.Content,
                SentAt = message.SentAt,
                UserId = message.UserId,
                EventId = message.EventId,
                Username = user?.Username
            };
        }
    }

}
