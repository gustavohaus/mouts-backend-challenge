using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;
    private readonly Faker _faker;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IValidator<CancelSaleCommand> _validator;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _validator = Substitute.For<IValidator<CancelSaleCommand>>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _logger, _validator);
    }

    [Fact(DisplayName = "Given a valid sale When cancelling Then sale is cancelled successfully")]
    public async Task Handle_ValidSale_CancelsSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var command = new CancelSaleCommand { SaleId = sale.Id };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CancelSaleResult>(Arg.Any<Sale>()).Returns(new CancelSaleResult { Id = sale.Id });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.SaleProducts.All(p => p.IsCancelled).Should().BeTrue();
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given a non-existent sale When cancelling Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CancelSaleCommand { SaleId = Guid.NewGuid() };
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.SaleId} not found");
    }

    [Fact(DisplayName = "Given a sale with products eligible for discounts When cancelling Then discounts are recalculated")]
    public async Task Handle_ProductsEligibleForDiscounts_RecalculatesDiscounts()
    {
        // Arrange
        var sale = GenerateValidSale();
        var command = new CancelSaleCommand { SaleId = sale.Id };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.TotalAmount.Should().Be(0); // All products are cancelled
    }

    [Fact(DisplayName = "Given a sale with more than 20 items per product When cancelling Then throws InvalidOperationException")]
    public async Task Handle_ProductsExceeding20Items_ThrowsInvalidOperationException()
    {
        // Arrange
        var sale = GenerateValidSale();
        var product = sale.SaleProducts.First();
        product.Update(21); // Exceeding the limit
        var command = new CancelSaleCommand { SaleId = sale.Id };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 identical items.");
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

    private Product GenerateValidProduct()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };
    }
}