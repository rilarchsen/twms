using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.DTOs;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserDbContext _dbContext;

        public UsersController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // GET: Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _dbContext.Users.ToListAsync());
        }
        
        // GET: Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(Guid id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        // POST: Users
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser is not null)
                return Conflict("User already exists");
            var newUser = _dbContext.Users.Add(new User
            {
                Name = request.Name,
                Email = request.Email
            });
            await _dbContext.SaveChangesAsync();

            return Ok(newUser.Entity);
        }
        
        // PUT: Users
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(EditUserRequest request)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            
            if (user is null)
            {
                return NotFound();
            }

            user.Name = request.Name;
            user.Email = request.Email;
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }
        
        // DELETE: Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
