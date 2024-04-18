using Contracts.Products;
using Contracts.Users;
using MediatR;
using OrderModule.Data;

namespace OrderModule.BL;

internal interface IOrderManager
{
    Task MakeOrder(Guid userId, Guid productId);
}

internal class OrderManager : IOrderManager
{
    private IMediator _mediator;
    private IOrderRepository _repository;

    public OrderManager(IMediator mediator, IOrderRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task MakeOrder(Guid userId, Guid productId)
    {
        var userTask = _mediator.Send(new GetUserRequest { UserId = userId });
        var product = await _mediator.Send(new GetProductRequest { ProductId = productId });
        var user = await userTask;
        
        throw new NotImplementedException();
    }
}