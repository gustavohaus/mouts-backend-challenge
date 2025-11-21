using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Profiles;

public class CreateSaleProfilerTests
{
    private readonly IMapper _mapper;
    private readonly Faker _faker;

    public CreateSaleProfilerTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<CreateSaleProfiler>());
        _mapper = configuration.CreateMapper();
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a Sale entity When mapped to CreateSaleResult Then mapping is valid")]
    public void Map_SaleToCreateSaleResult_ShouldBeValid()
    {
        // Arrange
        var sale = GenerateValidSale();

        // Act
        var result = _mapper.Map<CreateSaleResult>(sale);

        // Assert
        result.Should().NotBeNull();
        result.SaleNumber.Should().Be(sale.SaleNumber);
        result.SaleDate.Should().Be(sale.CreatedAt);
        result.SaleProducts.Should().HaveCount(sale.SaleProducts.Count);
    }

    [Fact(DisplayName = "Given a SaleProduct entity When mapped to CreateSaleProductResult Then mapping is valid")]
    public void Map_SaleProductToCreateSaleProductResult_ShouldBeValid()
    {
        // Arrange
        var saleProduct = GenerateValidSaleProduct();

        // Act
        var result = _mapper.Map<CreateSaleProductResult>(saleProduct);

        // Assert
        result.Should().NotBeNull();
        result.UnitPrice.Should().Be(Math.Round(saleProduct.Product.UnitPrice,2));
        result.DiscountPercent.Should().Be(saleProduct.DiscountPercent);
    }

    private Sale GenerateValidSale()
    {
        var branch = new Branch { Id = _faker.Random.Guid(), Status = BranchStatus.Active };
        var customer = new User { Id = _faker.Random.Guid(), Role = UserRole.Customer };
        var sale = new Sale(_faker.Random.String2(10), branch, customer);

        var products = Enumerable.Range(1, _faker.Random.Int(1, 5))
            .Select(_ => new SaleProduct(sale, GenerateValidProduct(), _faker.Random.Int(1, 10)))
            .ToList();

        sale.AddProducts(products);
        return sale;
    }

    private SaleProduct GenerateValidSaleProduct()
    {
        var sale = GenerateValidSale();
        var product = GenerateValidProduct();
        return new SaleProduct(sale, product, _faker.Random.Int(1, 10));
    }

    private Product GenerateValidProduct()
    {
        return new Product
        {
            Id = _faker.Random.Guid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };
    }
}