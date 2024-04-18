using ProductModule.Data;

namespace ProductModule.BL;

internal interface IProductManager
{
    IList<ProductDto> GetProducts();

    ProductDto? GetProduct(Guid id);

    void AddProduct(string name, int quantity);

    bool OrderProduct(Guid id, int quantity);
}

internal record ProductDto(Guid Id, string Name, int Quantity);

internal class ProductManager(IProductRepository repository) : IProductManager
{
    public IList<ProductDto> GetProducts()
    {
        return repository.GetAll().Select(p => new ProductDto(p.Id, p.Name, p.Quantity)).ToList();
    }

    public ProductDto? GetProduct(Guid id)
    {
        var result = repository.GetById(id);

        return result != null ? new ProductDto(result.Id, result.Name, result.Quantity) : null;
    }

    public void AddProduct(string name, int quantity)
    {
        repository.Add(new Product(Guid.NewGuid(), name, quantity));
    }

    public bool OrderProduct(Guid id, int quantity)
    {
        var product = repository.GetById(id);

        if (product == null || product.Quantity < quantity)
        {
            return false;
        }

        var originalQuantity = product.Quantity;
        repository.Update(product with { Quantity = originalQuantity - quantity });

        return true;
    }
}