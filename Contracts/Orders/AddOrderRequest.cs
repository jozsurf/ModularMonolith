using MediatR;

namespace Contracts.Orders;

public class AddOrderRequest : IRequest<Order>
{  
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}