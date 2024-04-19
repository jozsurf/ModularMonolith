using Contracts;
using Contracts.Products;
using Contracts.Users;
using MediatR;
using Moq;
using OrderModule.BL;
using OrderModule.Data;

namespace OrderModuleTests;

[TestFixture]
public class OrderManagerTests
{
    private IOrderManager _systemUnderTest;

    private Mock<IOrderRepository> _mockRepository;
    private Mock<IMediator> _mockMediator;

    private readonly User _user = new(Guid.NewGuid(), "Vratislav", "Gresko");
    private readonly Product _product = new(Guid.NewGuid(), "8BitDo Retro Mechanical Keyboard", 5);

    [SetUp]
    public void SetUp()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMediator.Setup(x => x.Send(It.Is<GetUserRequest>(u => u.UserId == _user.Id), default))
            .ReturnsAsync(_user);
        _mockMediator.Setup(x => x.Send(It.Is<GetProductRequest>(p => p.ProductId == _product.Id), default))
            .ReturnsAsync(_product);
        _mockMediator
            .Setup(x => x.Send(It.Is<OrderProductRequest>(o => o.ProductId == _product.Id && o.Quantity == 1), default))
            .ReturnsAsync(new Result(true));
        
        _mockRepository = new Mock<IOrderRepository>();
        _mockRepository.Setup(r => r.Add(new Order(It.IsAny<Guid>(), _user.Id, _product.Id, It.IsAny<DateTime>())));
        _mockRepository.Setup(r => r.GetById(It.IsAny<Guid>()))
            .Returns(new Order(It.IsAny<Guid>(), _user.Id, _product.Id, It.IsAny<DateTime>()));
        
        _systemUnderTest = new OrderManager(_mockMediator.Object, _mockRepository.Object);
    }

    [Test]
    public async Task MakeOrder_WhenUserIdAndProductIdAreValid_OrderCreated()
    {
        var result = await _systemUnderTest.MakeOrder(_user.Id, _product.Id);

        Assert.NotNull(result);
        _mockRepository.Verify(r => r.Add(It.Is<Order>(o => o.UserId == _user.Id && o.ProductId == _product.Id)),
            Times.Once);
    }

    [Test]
    public void MakeOrder_WhenUserIdInvalid_ExceptionThrown()
    {
        _mockMediator.Setup(x => x.Send(It.Is<GetUserRequest>(u => u.UserId == _user.Id), default))
            .ReturnsAsync((User?)null);

        Assert.ThrowsAsync<ApplicationException>(async () => await _systemUnderTest.MakeOrder(_user.Id, _product.Id));
    }
    
    [Test]
    public void MakeOrder_WhenProductIdInvalid_ExceptionThrown()
    {
        _mockMediator.Setup(x => x.Send(It.Is<GetProductRequest>(u => u.ProductId == _product.Id), default))
            .ReturnsAsync((Product?)null);

        Assert.ThrowsAsync<ApplicationException>(async () => await _systemUnderTest.MakeOrder(_user.Id, _product.Id));
    }

    [Test]
    public void MakeOrder_WhenOrderProductRequestFails_ExceptionThrown()
    {
        _mockMediator
            .Setup(x => x.Send(It.Is<OrderProductRequest>(o => o.ProductId == _product.Id && o.Quantity == 1), default))
            .ReturnsAsync(new Result(false));

        Assert.ThrowsAsync<ApplicationException>(async () => await _systemUnderTest.MakeOrder(_user.Id, _product.Id));
    }
}