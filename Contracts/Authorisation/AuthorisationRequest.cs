using MediatR;

namespace Contracts.Authorisation;

public class AuthorisationRequest : IRequest<AuthorisedUser?>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}