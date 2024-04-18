using Contracts.Users;
using MediatR;
using UserModule.BL;

namespace UserModule.Mediator;

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