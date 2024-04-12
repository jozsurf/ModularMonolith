using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModularMonolithApi.Models;
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

app.MapGet("/api/user/{userId}",
        async (Guid userId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserRequest { UserId = userId });

            if (result != null)
            {
                return Results.Ok(result);
            }

            return Results.NotFound(result);
        })
    .WithName("GetUser")
    .WithOpenApi();

app.MapPost("/api/user",
        async (CreateUserModel model, [FromServices] IMediator mediator) =>
        await mediator.Send(new AddUserRequest { FirstName = model.FirstName, Surname = model.Surname }))
    .WithName("AddUser")
    .WithOpenApi();

app.Run();