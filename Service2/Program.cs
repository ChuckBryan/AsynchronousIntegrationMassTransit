using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string host = builder.Configuration["RABBITMQ_HOST"] ??
              throw new InvalidOperationException("RABBITMQ_HOST configuration is invalid");
string port = builder.Configuration["RABBITMQ_PORT"]??
              throw new InvalidOperationException("RABBITMQ_PORT configuration is invalid");
string pass = builder.Configuration["RABBITMQ_PASS"] ??
              throw new InvalidOperationException("RABBITMQ_USER configuration is invalid");
string user = builder.Configuration["RABBITMQ_USER"]??
              throw new InvalidOperationException("RABBITMQ_PASS configuration is invalid");

// Add health check services: https://www.nuget.org/packages/AspNetCore.HealthChecks.Rabbitmq/
builder.Services
    .AddHealthChecks()
    .AddRabbitMQ(rabbitConnectionString:$"amqp://{user}:{pass}@{host}:{port}/");
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map a health endpoint
app.MapHealthChecks("/health").WithName("Health").WithOpenApi();

app.Run();