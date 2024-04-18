namespace Contracts.Orders;

public record Order(Guid Id, Guid UserId, Guid ProductId, DateTime OrderedOn);