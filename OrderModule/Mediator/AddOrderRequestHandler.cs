using Contracts.Orders;
using MediatR;
using OrderModule.BL;

namespace OrderModule.Mediator;

internal class AddOrderRequestHandler(IOrderManager orderManager) : IRequestHandler<AddOrderRequest, Order>
{
    public async Task<Order> Handle(AddOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await orderManager.MakeOrder(request.UserId, request.ProductId);

        return new Order(order.Id, order.UserId, order.ProductId, order.OrderedOn);
    }
}