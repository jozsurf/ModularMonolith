using System.Security.Claims;
using Contracts.Orders;
using Contracts.Products;
using Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ModularMonolithApi.Models;
using OrderModule;
using ProductModule;
using UserModule;
using AuthorisationModule;
using Contracts.Authorisation;

const string authenticationSchema = "bearer";

const string adminPolicyName = "admin";
const string supervisorPolicyName = "superv";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            []
        }
    });
});

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UserModule.DependencyInjectionHelper).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ProductModule.DependencyInjectionHelper).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(OrderModule.DependencyInjectionHelper).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AuthorisationModule.DependencyInjectionHelper).Assembly);
});

builder.Services.RegisterUserModuleDependencies();
builder.Services.RegisterProductModuleDependencies();
builder.Services.RegisterOrderModuleDependencies();
builder.Services.RegisterAuthorisationModuleDependencies();

builder.Services.AddAuthentication().AddBearerToken(authenticationSchema);
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(adminPolicyName, p => p.RequireClaim("role", "admin"));
    opt.AddPolicy(supervisorPolicyName, p => p.RequireClaim("role", "admin", "super"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/user", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetUsersRequest());
    return Results.Ok(result);
})
    .WithName("GetUsers")
    .WithTags("User")
    .RequireAuthorization(supervisorPolicyName)
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
    .RequireAuthorization(supervisorPolicyName)
    .WithOpenApi();

app.MapPost("/api/user",
        async (CreateUserModel model, [FromServices] IMediator mediator) =>
        await mediator.Send(new AddUserRequest { FirstName = model.FirstName, Surname = model.Surname }))
    .WithName("AddUser")
    .WithTags("User")
    .RequireAuthorization(adminPolicyName)
    .WithOpenApi();

app.MapGet("/api/product", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetProductsRequest());
    return Results.Ok(result);
})
    .WithName("GetProducts")
    .WithTags("Product")
    .RequireAuthorization(supervisorPolicyName)
    .WithOpenApi();

app.MapGet("/api/order/{orderId}", async (Guid orderId, [FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetOrderRequest { Id = orderId });

        if (result != null)
        {
            return Results.Ok(result);
        }

        return Results.NotFound(result);
    })
    .WithName("GetOrder")
    .WithTags("Order")
    .RequireAuthorization(supervisorPolicyName)
    .WithOpenApi();

app.MapPost("/api/order",
        async (AddOrderModel model, [FromServices] IMediator mediator) =>
        await mediator.Send(new AddOrderRequest { UserId = model.UserId, ProductId = model.ProductId }))
    .WithName("AddOrder")
    .WithTags("Order")
    .RequireAuthorization(adminPolicyName)
    .WithOpenApi();

app.MapPost("/api/auth", async (AuthorisationModel model, IMediator mediator) =>
    {
        var authorisedUser = await mediator.Send(new AuthorisationRequest
            { Username = model.Username, Password = model.Password });

        if (authorisedUser == null)
        {
            return Results.Unauthorized();
        }

        return Results.SignIn(new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim("username", authorisedUser.Username),
                    new Claim("role", authorisedUser.Roles.FirstOrDefault() ?? string.Empty)
                }, authenticationSchema)),
            authenticationScheme: authenticationSchema);
    })
    .WithName("Authenticate")
    .WithTags("Auth")
    .AllowAnonymous()
    .WithOpenApi();

app.Run();