using Proculite.GpioRest.Middlewares;
using Proculite.GpioRest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<GpioService>();

var app = builder.Build();

// Custom security. Expects traffic to be secure (HTTPS).
app.UseSecurity();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();
