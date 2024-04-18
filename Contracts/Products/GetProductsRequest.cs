using MediatR;

namespace Contracts.Products;

public class GetProductsRequest : IRequest<List<Product>>
{
    
}