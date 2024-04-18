using System.Collections.Concurrent;

namespace OrderModule.Data;

internal interface IOrderRepository
{
    IList<Order> GetAll();
    Order? GetById(Guid id);
    void Add(Order product);
    void Update(Order product);
    void Delete(Guid id);
}

internal record Order(Guid Id, Guid UserId, Guid ProductId, DateTime OrderedOn);

internal class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _repository = new();
    
    public IList<Order> GetAll()
    {
        return _repository.Values.ToList();
    }

    public Order? GetById(Guid id)
    {
        return _repository.GetValueOrDefault(id);
    }

    public void Add(Order product)
    {
        if (!_repository.TryAdd(product.Id, product))
            throw new DataException();
    }

    public void Update(Order product)
    {
        var existingValue = GetById(product.Id);

        if (existingValue == null || !_repository.TryUpdate(product.Id, product, existingValue))
            throw new DataException();
    }

    public void Delete(Guid id)
    {
        if (!_repository.TryRemove(id, out var _))
            throw new DataException();
    }
}