using UserModule.Data;

namespace UserModule.BL;

internal interface IUserManager
{
    UserDto? GetUser(Guid id);

    UserDto AddUser(string firstName, string surname);
}

internal record UserDto(Guid Id, string FirstName, string Surname);

internal class UserManager(IUserRepository repository) : IUserManager
{
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