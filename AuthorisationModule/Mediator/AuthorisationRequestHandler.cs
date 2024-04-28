using AuthorisationModule.BL;
using Contracts.Authorisation;
using MediatR;

namespace AuthorisationModule.Mediator;

internal class AuthorisationRequestHandler : IRequestHandler<AuthorisationRequest, AuthorisedUser?>
{
    private readonly IAuthorisationManager _authorisationManager;

    public AuthorisationRequestHandler(IAuthorisationManager authorisationManager)
    {
        _authorisationManager = authorisationManager;
    }

    public Task<AuthorisedUser?> Handle(AuthorisationRequest request, CancellationToken cancellationToken)
    {
        var result = _authorisationManager.Authorise(request.Username, request.Password);

        if (!result.Authorised)
        {
            return Task.FromResult((AuthorisedUser?)null);
        }

        return Task.FromResult((AuthorisedUser?)new AuthorisedUser(result.User!.Username, result.User.Roles));
    }
}