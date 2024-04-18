using MediatR;

namespace Contracts.Users;

public class GetUserRequest : IRequest<User?>
{
    public Guid UserId { get; set; }
}