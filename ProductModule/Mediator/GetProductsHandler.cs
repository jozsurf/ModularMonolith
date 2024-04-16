using MediatR;
using ProductModule.BL;

namespace ProductModule.Mediator;

public record Product(Guid Id, string Name, int Quantity);

public class GetProductsRequest : IRequest<List<Product>>
{
    
}

internal class GetProductsHandler(IProductManager productManager) : IRequestHandler<GetProductsRequest, List<Product>>
{
    public Task<List<Product>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var products = productManager.GetProducts();

        return Task.FromResult(products.Select(p => new Product(p.Id, p.Name, p.Quantity)).ToList());
    }
}