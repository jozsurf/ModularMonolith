using MediatR;
using UserModule.BL;

namespace UserModule.Mediator;

public record User(Guid Id, string FirstName, string Surname);

public class GetUserRequest : IRequest<User?>
{
    public Guid UserId { get; set; }
}

internal class GetUserHandler(IUserManager userManager) : IRequestHandler<GetUserRequest, User?>
{
    public Task<User?> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = userManager.GetUser(request.UserId);

        if (user != null)
        {
            return Task.FromResult((User?) new User(user.Id, user.FirstName, user.Surname));
        }

        return Task.FromResult((User?)null);
    }
}