using Contracts.Orders;
using Contracts.Products;
using Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModularMonolithApi.Models;
using OrderModule;
using ProductModule;
using UserModule;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UserModule.DependencyInjectionHelper).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ProductModule.DependencyInjectionHelper).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(OrderModule.DependencyInjectionHelper).Assembly);
});

builder.Services.RegisterUserModuleDependencies();
builder.Services.RegisterProductModuleDependencies();
builder.Services.RegisterOrderModuleDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/user", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetUsersRequest());
    return Results.Ok(result);
})
    .WithName("GetUsers")
    .WithTags("User")
    .WithOpenApi();

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
    .WithTags("User")
    .WithOpenApi();

app.MapPost("/api/user",
        async (CreateUserModel model, [FromServices] IMediator mediator) =>
        await mediator.Send(new AddUserRequest { FirstName = model.FirstName, Surname = model.Surname }))
    .WithName("AddUser")
    .WithTags("User")
    .WithOpenApi();

app.MapGet("/api/product", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetProductsRequest());
    return Results.Ok(result);
})
    .WithName("GetProducts")
    .WithTags("Product")
    .WithOpenApi();

app.MapPost("/api/order",
        async (AddOrderModel model, [FromServices] IMediator mediator) =>
        await mediator.Send(new AddOrderRequest { UserId = model.UserId, ProductId = model.ProductId }))
    .WithName("AddOrder")
    .WithTags("Order")
    .WithOpenApi();

app.Run();