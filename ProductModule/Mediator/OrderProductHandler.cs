using Contracts;
using Contracts.Products;
using MediatR;
using ProductModule.BL;

namespace ProductModule.Mediator;

internal class OrderProductHandler : IRequestHandler<OrderProductRequest, Result>
{
    private readonly IProductManager _productManager;

    public OrderProductHandler(IProductManager productManager)
    {
        _productManager = productManager;
    }

    public Task<Result> Handle(OrderProductRequest request, CancellationToken cancellationToken)
    {
        var result = _productManager.OrderProduct(request.ProductId, request.Quantity);

        return result
            ? Task.FromResult(new Result(true))
            : Task.FromResult(new Result(false,
                $"Unable to order {request.Quantity} of product with ID {request.ProductId}"));
    }
}