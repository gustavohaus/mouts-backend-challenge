//using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
//using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
//using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.Domain.Enums;
//using Ambev.DeveloperEvaluation.Domain.Repositories;
//using AutoMapper;
//using Bogus;
//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using NSubstitute;
//using Xunit;

//namespace Ambev.DeveloperEvaluation.Unit.Application;

//public class UpdateSaleHandlerTests
//{
//    private readonly ISaleRepository _saleRepository;
//    private readonly IMapper _mapper;
//    private readonly ILogger<UpdateSaleHandler> _logger;
//    private readonly UpdateSaleHandler _handler;
//    private readonly Faker _faker;

//    public UpdateSaleHandlerTests()
//    {
//        _saleRepository = Substitute.For<ISaleRepository>();
//        _mapper = Substitute.For<IMapper>();
//        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
//        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _logger);
//        _faker = new Faker();
//    }

//    [Fact(DisplayName = "Given valid update data When updating sale Then updates successfully")]
//    public async Task Handle_ValidUpdateData_UpdatesSuccessfully()
//    {
//        // Arrange
//        var sale = GenerateValidSale();
//        var command = GenerateValidUpdateSaleCommand(sale.Id);

//        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
//        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
//        _mapper.Map<GetSaleResult>(Arg.Any<Sale>()).Returns(new GetSaleResult { Id = sale.Id });

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.Id.Should().Be(sale.Id);
//        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
//    }

//    [Fact(DisplayName = "Given non-existent sale When updating Then throws KeyNotFoundException")]
//    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
//    {
//        // Arrange
//        var command = GenerateValidUpdateSaleCommand(Guid.NewGuid());
//        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale)null);

//        // Act
//        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        await act.Should().ThrowAsync<KeyNotFoundException>()
//            .WithMessage($"User with ID {command.Id} not found");
//    }

//    [Fact(DisplayName = "Given a sale with products exceeding 20 items When updating Then throws InvalidOperationException")]
//    public async Task Handle_ProductsExceeding20Items_ThrowsInvalidOperationException()
//    {
//        // Arrange
//        var sale = GenerateValidSale();
//        var command = GenerateValidUpdateSaleCommand(sale.Id);
//        command.SaleProducts.First().Quantity = 21;

//        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);

//        // Act
//        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        await act.Should().ThrowAsync<InvalidOperationException>()
//            .WithMessage("Cannot sell more than 20 identical items.");
//    }

//    [Fact(DisplayName = "Given a sale with products eligible for 10% discount When updating Then applies discount")]
//    public async Task Handle_ProductsEligibleFor10PercentDiscount_AppliesDiscount()
//    {
//        // Arrange
//        var sale = GenerateValidSale();
//        var command = GenerateValidUpdateSaleCommand(sale.Id);
//        command.SaleProducts.First().Quantity = 5;

//        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
//        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        sale.SaleProducts.First().DiscountPercent.Should().Be(10.0m);
//    }

//    [Fact(DisplayName = "Given a sale with products eligible for 20% discount When updating Then applies discount")]
//    public async Task Handle_ProductsEligibleFor20PercentDiscount_AppliesDiscount()
//    {
//        // Arrange
//        var sale = GenerateValidSale();
//        var command = GenerateValidUpdateSaleCommand(sale.Id);
//        command.SaleProducts.First().Quantity = 15;

//        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
//        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        sale.SaleProducts.First().DiscountPercent.Should().Be(20.0m);
//    }

//    private Sale GenerateValidSale()
//    {
//        var branch = new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active };
//        var customer = new User { Id = Guid.NewGuid(), Role = UserRole.Customer };
//        var sale = new Sale(_faker.Random.String2(10), branch, customer);

//        var products = Enumerable.Range(1, _faker.Random.Int(1, 5))
//            .Select(_ => new SaleProduct(sale, GenerateValidProduct(), _faker.Random.Int(1, 10)))
//            .ToList();

//        sale.AddProducts(products);
//        return sale;
//    }

//    private Product GenerateValidProduct()
//    {
//        return new Product
//        {
//            Id = Guid.NewGuid(),
//            UnitPrice = _faker.Random.Decimal(10, 100)
//        };
//    }

//    private UpdateSaleCommand GenerateValidUpdateSaleCommand(Guid saleId)
//    {
//        return new UpdateSaleCommand
//        {
//            Id = saleId,
//            SaleNumber = _faker.Random.String2(10),
//            SaleProducts = new List<UpdateSaleProductsCommand>
//            {
//                new()
//                {
//                    ProductId = Guid.NewGuid(),
//                    Quantity = _faker.Random.Int(1, 10),
//                    UnitPrice = _faker.Random.Decimal(10, 100)
//                }
//            }
//        };
//    }
//}