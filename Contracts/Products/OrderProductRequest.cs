using MediatR;

namespace Contracts.Products;

public class OrderProductRequest : IRequest<Result>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}