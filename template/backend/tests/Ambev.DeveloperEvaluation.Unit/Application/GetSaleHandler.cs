using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;
    private readonly GetSaleHandler _handler;
    private readonly Faker _faker;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetSaleHandler>>();
        _handler = new GetSaleHandler(_saleRepository, _mapper, _logger);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a valid sale ID When retrieving sale Then returns sale successfully")]
    public async Task Handle_ValidSaleId_ReturnsSaleSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var command = new GetSaleCommand(sale.Id);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(new GetSaleResult { Id = sale.Id });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given a non-existent sale ID When retrieving sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSaleId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new GetSaleCommand(Guid.NewGuid());
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.SaleId} not found.");
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