using backend.Data;
using backend.DTOs;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net; 

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [Authorize(Roles = "Examiner")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username
                })
                .ToListAsync();
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "Examiner")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        [HttpPost]
        [Authorize(Roles = "Examiner")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
          
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                Username = createUserDto.Username,
                Password = hashedPassword, 
                Role = createUserDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Username = user.Username
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Examiner")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Username = updateUserDto.Username;
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password); 
            }
            user.Role = updateUserDto.Role;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Examiner")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
