using TimeWorkedManagementSystem;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.RegisterMiddleware();

app.Run();
