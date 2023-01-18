using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Middleware
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly IUserService _userService;
        private readonly UserDbContext _dbContext;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(IUserService userService, UserDbContext dbContext,
        ILogger<AuthorizationMiddleware> logger)
        {
            _userService = userService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? name = context.User.Identity?.Name;
            string? email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email is not null)
            {
                User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user is null)
                {
                    user = new User { Name = name ?? "Unidentified Quack", Email = email };
                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();
                }

                _userService.SetUserId(user.Id);

                _logger.LogInformation($"Set user ID: {user.Id}");
            } else
            {
                _logger.LogInformation("Unidentified user", context);
            }

            await next(context);
        }
    }
}
