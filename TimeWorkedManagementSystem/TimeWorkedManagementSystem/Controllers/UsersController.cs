using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.DTOs;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly UserDbContext _dbContext;
        private readonly IUserService _userService;

        public UsersController(UserDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }
        
        // GET: Users
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(User[]))]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _dbContext.Users.ToListAsync());
        }
        
        // GET: Users/Current
        [HttpGet("Current")]
        [SwaggerResponse(200, "OK", typeof(User))]        
        [SwaggerResponse(404, "Not Found")]
        public async Task<IActionResult> GetCurrentUserDetails()
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _userService.UserId);
            
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        // GET: Users/{id}
        [HttpGet("{id}")]
        [SwaggerResponse(200, "OK", typeof(User))]        
        [SwaggerResponse(404, "Not Found")]
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
        [SwaggerResponse(200, "OK", typeof(User))]
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
        [SwaggerResponse(200, "OK", typeof(User))]        
        [SwaggerResponse(404, "Not Found")]
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
        [SwaggerResponse(200, "OK")]        
        [SwaggerResponse(404, "Not Found")]
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
