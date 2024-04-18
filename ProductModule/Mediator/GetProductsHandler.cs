using Contracts.Products;
using MediatR;
using ProductModule.BL;

namespace ProductModule.Mediator;

internal class GetProductsHandler(IProductManager productManager) : IRequestHandler<GetProductsRequest, List<Product>>
{
    public Task<List<Product>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var products = productManager.GetProducts();

        return Task.FromResult(products.Select(p => new Product(p.Id, p.Name, p.Quantity)).ToList());
    }
}