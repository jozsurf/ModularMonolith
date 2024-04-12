using MediatR;
using UserModule.BL;

namespace UserModule.Mediator;

public class AddUserRequest : IRequest<User>
{
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
}

internal class AddUserHandler(IUserManager userManager) : IRequestHandler<AddUserRequest, User>
{
    public Task<User> Handle(AddUserRequest request, CancellationToken cancellationToken)
    {
        var user = userManager.AddUser(request.FirstName, request.Surname);

        return Task.FromResult(new User(user.Id, user.FirstName, user.Surname));
    }
}