using MediatR;

namespace Contracts.Users;

public class AddUserRequest : IRequest<User>
{
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
}