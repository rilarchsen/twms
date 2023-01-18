using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
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

            builder.Services
                .AddAuth0WebAppAuthentication(options => {
                    options.Domain = builder.Configuration["Auth0:Domain"];
                    options.ClientId = builder.Configuration["Auth0:ClientId"];
                    options.Scope += " email";
                    //options.CallbackPath = "/account/profile";
                });

            return builder;
        }
    }
}
