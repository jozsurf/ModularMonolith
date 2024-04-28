namespace Contracts.Authorisation;

public record AuthorisedUser (string Username, List<string> Roles);