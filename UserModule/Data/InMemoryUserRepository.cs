using System.Collections.Concurrent;

namespace UserModule.Data;

internal interface IUserRepository
{
    User? GetById(Guid id);
    void Add(User user);
    void Update(User user);
    void Delete(Guid id);
}

internal record User(Guid Id, string FirstName, string Surname);

internal class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();
    
    public User? GetById(Guid id)
    {
        return _users.GetValueOrDefault(id);
    }

    public void Add(User user)
    {
        if (!_users.TryAdd(user.Id, user))
            throw new DataException();
    }

    public void Update(User user)
    {
        var existingUser = GetById(user.Id);
        
        if (existingUser == null || !_users.TryUpdate(user.Id, user, existingUser))
            throw new DataException();
    }

    public void Delete(Guid id)
    {
        if (!_users.TryRemove(id, out _))
            throw new DataException();
    }
}