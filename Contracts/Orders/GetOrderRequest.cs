using MediatR;

namespace Contracts.Orders;

public class GetOrderRequest : IRequest<Order?>
{
    public Guid Id { get; set; }
}