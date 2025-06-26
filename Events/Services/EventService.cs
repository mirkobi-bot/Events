using Events.Data;
using Events.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Events.Services
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> SaveEventAsync(EventDto e, ClaimsPrincipal u);
        Task<Event> GetEventByIdAsync(int eventId);
        Task<bool> DeleteEventAsync(int eventId);


    }
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }

        public async Task<Event?> SaveEventAsync(EventDto e, ClaimsPrincipal u)
        {
            //Recupera l'userId dal claim
            var userIdClaim = u.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return null;

            var userId = int.Parse(userIdClaim);

            if (e.Id == 0)
            {
                var newEvent = new Event
                {
                    Title = e.Title,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedByUserId = userId
                };

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return newEvent;
            }

            var existingEvent = await _context.Events.FindAsync(e.Id);
            if (existingEvent == null)
                return null;

            // Optional: Verifica che l'utente possa modificare quell'evento
            if (existingEvent.CreatedByUserId != userId)
                return null; // oppure throw Unauthorized

            existingEvent.Title = e.Title;
            existingEvent.Description = e.Description;
            existingEvent.StartDate = e.StartDate;
            existingEvent.EndDate = e.EndDate;

            _context.Events.Update(existingEvent);
            await _context.SaveChangesAsync();
            return existingEvent;
        }


        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var existingEvent = await _context.Events.FindAsync(eventId);
            if (existingEvent == null)
                return false;

            _context.Events.Remove(existingEvent);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
