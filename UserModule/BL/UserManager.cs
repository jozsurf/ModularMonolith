using UserModule.Data;

namespace UserModule.BL;

internal interface IUserManager
{
    UserDto? GetUser(Guid id);

    void AddUser(UserDto user);
}

internal record UserDto(Guid Id, string FirstName, string Surname);

internal class UserManager(IUserRepository repository) : IUserManager
{
    public UserDto? GetUser(Guid id)
    {
        var result = repository.GetById(id);

        return result != null ? new UserDto(result.Id, result.FirstName, result.Surname) : null;
    }

    public void AddUser(UserDto user)
    {
        repository.Add(new User(user.Id, user.FirstName, user.Surname));
    }
}