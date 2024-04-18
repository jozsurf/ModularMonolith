using Contracts.Products;
using Contracts.Users;
using MediatR;
using OrderModule.Data;

namespace OrderModule.BL;

internal interface IOrderManager
{
    Task<OrderDto> MakeOrder(Guid userId, Guid productId);
}

internal record OrderDto(Guid Id, Guid UserId, Guid ProductId, DateTime OrderedOn);

internal class OrderManager : IOrderManager
{
    private readonly IMediator _mediator;
    private readonly IOrderRepository _repository;

    public OrderManager(IMediator mediator, IOrderRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<OrderDto> MakeOrder(Guid userId, Guid productId)
    {
        var userTask = _mediator.Send(new GetUserRequest { UserId = userId });
        var product = await _mediator.Send(new GetProductRequest { ProductId = productId });
        var user = await userTask;

        if (product == null || user == null)
            throw new ApplicationException("Product or User cannot be located");

        var productOrderResult = await _mediator.Send(new OrderProductRequest { ProductId = productId, Quantity = 1 });
        if (!productOrderResult.Success)
            throw new ApplicationException(productOrderResult.Message);
            
        var orderId = Guid.NewGuid();
        _repository.Add(new Order(orderId, user.Id, product.Id, DateTime.UtcNow));

        var order = _repository.GetById(orderId)!;
        return new OrderDto(order.Id, order.UserId, order.ProductId, order.OrderedOn);
    }
}