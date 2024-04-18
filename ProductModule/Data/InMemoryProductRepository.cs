using System.Collections.Concurrent;

namespace ProductModule.Data;

internal interface IProductRepository
{
    IList<Product> GetAll();
    Product? GetById(Guid id);
    void Add(Product product);
    void Update(Product product);
    void Delete(Guid id);
}

internal class DataException : Exception{}

internal record Product(Guid Id, string Name, int Quantity); 

internal class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products;

    public InMemoryProductRepository()
    {
        _products = new ();

        List<Product> initialProducts =
        [
            new Product(Guid.NewGuid(), "Das Keyboard", 10), 
            new Product(Guid.NewGuid(), "Logitech Marathon Mouse", 10),
            new Product(Guid.NewGuid(), "Philips 4K Ultra HD", 10)
        ];

        foreach (var product in initialProducts)
        {
            _products.TryAdd(product.Id, product);
        }
    }
    
    public IList<Product> GetAll()
    {
        return _products.Values.ToList();
    }

    public Product? GetById(Guid id)
    {
        return _products.GetValueOrDefault(id);
    }

    public void Add(Product product)
    {
        if (!_products.TryAdd(product.Id, product))
            throw new DataException();
    }

    public void Update(Product product)
    {
        var existingProduct = GetById(product.Id);

        if (existingProduct == null || !_products.TryUpdate(product.Id, product, existingProduct))
            throw new DataException();
    }

    public void Delete(Guid id)
    {
        if (!_products.TryRemove(id, out _))
            throw new DataException();
    }
}