using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserModule;
using UserModule.Mediator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionHelper).Assembly);
});

builder.Services.RegisterUserModuleDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/user/{userId}", async (Guid userId, [FromServices]IMediator  mediator) =>
{
    
    var result = await mediator.Send(new GetUserRequest { UserId = userId });

    return result;
})
    .WithName("GetUser")
    .WithOpenApi();

app.Run();