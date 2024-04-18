using Contracts.Products;
using MediatR;
using ProductModule.BL;

namespace ProductModule.Mediator;

internal class GetProductHandler(IProductManager productManager) : IRequestHandler<GetProductRequest, Product?>
{
    public Task<Product?> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var result = productManager.GetProduct(request.ProductId);

        if (result != null)
            return Task.FromResult((Product?)new Product(result.Id, result.Name, result.Quantity));
        
        return Task.FromResult((Product?)null);
    }
}