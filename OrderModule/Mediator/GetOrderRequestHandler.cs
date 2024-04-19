using Contracts.Orders;
using MediatR;
using OrderModule.BL;

namespace OrderModule.Mediator;

internal class GetOrderRequestHandler(IOrderManager orderManager) : IRequestHandler<GetOrderRequest, Order?>
{
    public Task<Order?> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var order = orderManager.GetOrderById(request.Id);

        return order != null
            ? Task.FromResult((Order?)new Order(order.Id, order.UserId, order.ProductId, order.OrderedOn))
            : Task.FromResult((Order?)null);
    }
}