using Wolverine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseWolverine();

var app = builder.Build();


app.Run();
