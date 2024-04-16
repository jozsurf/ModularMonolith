using UserModule.Data;

namespace UserModule.BL;

internal interface IUserManager
{
    List<UserDto> GetUsers();
    UserDto? GetUser(Guid id);

    UserDto AddUser(string firstName, string surname);
}

internal record UserDto(Guid Id, string FirstName, string Surname);

internal class UserManager(IUserRepository repository) : IUserManager
{
    public List<UserDto> GetUsers()
    {
        return repository.GetUsers().Select(u => new UserDto(u.Id, u.FirstName, u.Surname)).ToList();
    }

    public UserDto? GetUser(Guid id)
    {
        var result = repository.GetById(id);

        return result != null ? new UserDto(result.Id, result.FirstName, result.Surname) : null;
    }

    public UserDto AddUser(string firstName, string surname)
    {
        var userId = Guid.NewGuid();
        repository.Add(new User(userId, firstName, surname));

        var user = repository.GetById(userId)!;

        return new UserDto(user.Id, user.FirstName, user.Surname);
    }
}