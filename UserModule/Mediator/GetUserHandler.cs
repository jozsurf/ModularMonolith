using MediatR;
using UserModule.BL;

namespace UserModule.Mediator;

public record User(Guid Id, string FirstName, string Surname);

public class GetUserRequest : IRequest<User>
{
    public Guid UserId { get; set; }
}

internal class GetUserHandler : IRequestHandler<GetUserRequest, User?>
{
    private readonly IUserManager _userManager;

    public GetUserHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task<User?> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = _userManager.GetUser(request.UserId);

        if (user != null)
        {
            return Task.FromResult(new User(user.Id, user.FirstName, user.Surname));
        }

        return Task.FromResult((User?)null);
    }
}