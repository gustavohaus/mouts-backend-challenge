
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleProduct;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class CancelSaleProductHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CancelSaleProductHandler _handler;
    private readonly Faker _faker;
    private readonly ILogger<CancelSaleProductHandler> _logger;

    public CancelSaleProductHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleProductHandler>>();
        _handler = new CancelSaleProductHandler(_saleRepository, _mapper, _logger);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a valid sale and product When cancelling product Then product is cancelled successfully")]
    public async Task Handle_ValidSaleAndProduct_CancelsProductSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var productToCancel = sale.SaleProducts.First();
        var command = new CancelSaleProductCommand
        {
            SaleId = sale.Id,
            SaleProduct = productToCancel.ProductId
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        productToCancel.IsCancelled.Should().BeTrue();
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given a non-existent sale When cancelling product Then throws ValidationException")]
    public async Task Handle_NonExistentSale_ThrowsValidationException()
    {
        // Arrange
        var command = new CancelSaleProductCommand
        {
            SaleId = Guid.NewGuid(),
            SaleProduct = Guid.NewGuid()
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage($"*Sale {command.SaleId} not found.*");
    }

    [Fact(DisplayName = "Given a non-existent product in sale When cancelling product Then throws InvalidOperationException")]
    public async Task Handle_NonExistentProductInSale_ThrowsInvalidOperationException()
    {
        // Arrange
        var sale = GenerateValidSale();
        var command = new CancelSaleProductCommand
        {
            SaleId = sale.Id,
            SaleProduct = Guid.NewGuid() // Non-existent product ID
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Product with ID {command.SaleProduct} not found in the sale.");
    }

    [Fact(DisplayName = "Given a sale with products eligible for discounts When cancelling Then discounts are recalculated")]
    public async Task Handle_ProductsEligibleForDiscounts_RecalculatesDiscounts()
    {
        // Arrange
        var sale = GenerateValidSaleWithSpecificQuantities(10); // 10 items for discount validation
        var productToCancel = sale.SaleProducts.First();
        var command = new CancelSaleProductCommand
        {
            SaleId = sale.Id,
            SaleProduct = productToCancel.ProductId
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        productToCancel.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(sale.SaleProducts.Where(p => !p.IsCancelled).Sum(p => p.TotalAmount));
    }

    [Fact(DisplayName = "Given a sale When cancelling a product Then respects discount rules")]
    public async Task Handle_CancellingProduct_RespectsDiscountRules()
    {
        // Arrange
        var sale = GenerateValidSaleWithSpecificQuantities(4); // 4 items for discount validation
        var productToCancel = sale.SaleProducts.First();
        var command = new CancelSaleProductCommand
        {
            SaleId = sale.Id,
            SaleProduct = productToCancel.ProductId
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        productToCancel.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(sale.SaleProducts.Where(p => !p.IsCancelled).Sum(p => p.TotalAmount));
    }

    private Sale GenerateValidSale()
    {
        var branch = new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active };
        var customer = new User { Id = Guid.NewGuid(), Role = UserRole.Customer };
        var sale = new Sale(_faker.Random.String2(10), branch, customer);

        var products = Enumerable.Range(1, _faker.Random.Int(1, 5))
            .Select(_ => new SaleProduct(sale, GenerateValidProduct(), _faker.Random.Int(1, 10)))
            .ToList();

        sale.AddProducts(products);
        return sale;
    }

    private Sale GenerateValidSaleWithSpecificQuantities(int quantity)
    {
        var branch = new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active };
        var customer = new User { Id = Guid.NewGuid(), Role = UserRole.Customer };
        var sale = new Sale(_faker.Random.String2(10), branch, customer);

        var product = GenerateValidProduct();
        var saleProduct = new SaleProduct(sale, product, quantity);

        sale.AddProduct(saleProduct);
        return sale;
    }

    private Product GenerateValidProduct()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };
    }
}