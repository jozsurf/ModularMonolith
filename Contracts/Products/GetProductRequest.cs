using MediatR;

namespace Contracts.Products;

public class GetProductRequest : IRequest<Product?>
{
    public Guid ProductId { get; set; }
}