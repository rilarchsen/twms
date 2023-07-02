using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Middleware;
using TimeWorkedManagementSystem.Services;

namespace TimeWorkedManagementSystem
{
    public static class ServiceBuilder
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            ServiceDescriptor? userDbContext = builder.Services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

            builder.Services.AddDbContext<UserDbContext>(options => {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<AuthorizationMiddleware>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Auth0:Domain"];
                options.Audience = builder.Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            return builder;
        }
    }
}
