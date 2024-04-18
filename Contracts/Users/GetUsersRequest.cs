using MediatR;

namespace Contracts.Users;

public class GetUsersRequest : IRequest<List<User>>{}