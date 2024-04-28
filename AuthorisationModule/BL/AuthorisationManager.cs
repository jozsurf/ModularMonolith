using AuthorisationModule.Data;

namespace AuthorisationModule.BL;

internal interface IAuthorisationManager
{
    (bool Authorised, AuthorisedUserDto? User) Authorise(string username, string password);
}

internal record AuthorisedUserDto(string Username, List<string> Roles);

internal class AuthorisationManager : IAuthorisationManager
{
    private readonly IAuthorisationRepository _repository;

    public AuthorisationManager(IAuthorisationRepository repository)
    {
        _repository = repository;
    }

    public (bool, AuthorisedUserDto?) Authorise(string username, string password)
    {
        var user = _repository.GetByUsername(username);

        if (user == null)
        {
            return (false, null);
        }

        if (!password.Equals(user.Password, StringComparison.Ordinal))
        {
            return (false, new AuthorisedUserDto(user.Username, user.Roles));
        }

        return (true, new AuthorisedUserDto(user.Username, user.Roles));
    }
}