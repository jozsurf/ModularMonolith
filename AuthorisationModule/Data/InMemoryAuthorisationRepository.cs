using System.Collections.Concurrent;

namespace AuthorisationModule.Data;

internal interface IAuthorisationRepository
{
    AuthorisedUser? GetByUsername(string username);
}

internal record AuthorisedUser(string Username, string Password, List<string> Roles);

internal class InMemoryAuthorisationRepository : IAuthorisationRepository
{
    private readonly ConcurrentDictionary<string, AuthorisedUser> _repository;

    public InMemoryAuthorisationRepository()
    {
        _repository = new();
        _repository.TryAdd("admin", new AuthorisedUser("admin", "@dmin!", ["admin"]));
        _repository.TryAdd("superv", new AuthorisedUser("superv", "superv!", ["super"]));
    }

    public AuthorisedUser? GetByUsername(string username)
    {
        return _repository.GetValueOrDefault(username);
    }
}