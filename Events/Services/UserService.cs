using Events.Data;
using Events.Models;
using Events.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Events.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();

        Task<IEnumerable<Event>> GetEventsByUser(int userId);
        Task<User> CreateUserAsync(UserDto dto);
        Task<User> UpdateUserAsync(UserDto dto);
        Task<User> GetUserByIdAsync(int eventId);
        Task<bool> DeleteUserAsync(int eventId);
        Task<dynamic> LoginAsync(string username, string password, JwtService jwt);

    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByUser(int userId)
        {
            return await _context.Set<Event>()
                .FromSqlInterpolated($"EXEC GetUserEvents @UserId = {userId}")
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> CreateUserAsync(UserDto dto)
        {
            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Phone = dto.Phone
            };
               

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User?> UpdateUserAsync(UserDto dto)
        {
            var existingUser = await _context.Users.FindAsync(dto.Id);
            if (existingUser == null)
                return null;

            existingUser.Username = dto.Username;
            existingUser.Email = dto.Email;
            existingUser.Password = dto.Password;
            existingUser.Phone = dto.Phone;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
                return false;

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<dynamic> LoginAsync(string username, string password, JwtService jwt)
        {
            
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

                if (user == null) return null;
            var token = jwt.GenerateToken(user);

                return new { token, username = user.Username, userid = user.Id };


        }

    }
}
