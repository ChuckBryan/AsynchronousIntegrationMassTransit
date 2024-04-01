using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ServiceMessages;

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
    .AddRabbitMQ(rabbitConnectionString:$"amqp://{user}:{pass}@{host}");

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.UsingRabbitMq((context, rmqHost) =>
    {
        rmqHost.Host(host, rmqHostConfig =>
        {
            rmqHostConfig.Username(user);
            rmqHostConfig.Password(pass);
        });
       
        rmqHost.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map a health endpoint
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).WithName("Health").WithOpenApi();


app.MapPost("/publish", async (IBus busContext, RegistrationEventMessage message) =>
{
    
    // publish
    await busContext.Publish(message);

    return Results.Ok("Message published");

});
app.Run();
