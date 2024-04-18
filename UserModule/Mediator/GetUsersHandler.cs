using Contracts.Users;
using MediatR;
using UserModule.BL;

namespace UserModule.Mediator;

internal class GetUsersHandler : IRequestHandler<GetUsersRequest, List<User>>
{
    private readonly IUserManager _userManager;

    public GetUsersHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task<List<User>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userManager.GetUsers().Select(u => new User(u.Id, u.FirstName, u.Surname)).ToList());
    }
}