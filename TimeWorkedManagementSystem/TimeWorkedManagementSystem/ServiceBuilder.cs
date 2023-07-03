using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "_myCORS",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //ServiceDescriptor? userDbContext = builder.Services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

            builder.Services.AddDbContext<UserDbContext>(options => {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<AuthorizationMiddleware>();
            
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TWMS API",
                    Description = "An ASP.NET Core Web API for managing work shifts",
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme {
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    //Scheme = "Bearer",
                    Description = "Put JWT Bearer token on textbox below! (include 'Bearer ' keyword)",

                    Reference = new OpenApiReference {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {jwtSecurityScheme, Array.Empty<string>()}
                });
            });

            return builder;
        }
    }
}
