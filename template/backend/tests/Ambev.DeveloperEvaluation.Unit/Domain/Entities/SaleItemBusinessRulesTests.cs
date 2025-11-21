using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleProductBusinessRulesTests
{
    private readonly Faker _faker;

    public SaleProductBusinessRulesTests()
    {
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a sale product with less than 4 items When calculating total price Then no discount is applied")]
    public void Given_LessThan4Items_When_CalculatingTotalPrice_Then_NoDiscount()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };

        var sale = new Sale(
            saleNumber: _faker.Random.String2(10),
            branch: new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active },
            customer: new User { Id = Guid.NewGuid(), Role = UserRole.Customer }
        );

        var saleProduct = new SaleProduct(sale, product, _faker.Random.Int(1, 3));

        // Act
        var totalPrice = saleProduct.TotalAmount;

        // Assert
        totalPrice.Should().Be(product.UnitPrice * saleProduct.Quantity);
    }

    [Fact(DisplayName = "Given a sale product with 4 to 9 items When calculating total price Then 10% discount is applied")]
    public void Given_4To9Items_When_CalculatingTotalPrice_Then_10PercentDiscount()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };

        var sale = new Sale(
            saleNumber: _faker.Random.String2(10),
            branch: new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active },
            customer: new User { Id = Guid.NewGuid(), Role = UserRole.Customer }
        );

        var saleProduct = new SaleProduct(sale, product, _faker.Random.Int(4, 9));

        // Act
        var totalPrice = saleProduct.TotalAmount;

        // Assert
        totalPrice.Should().Be(product.UnitPrice * saleProduct.Quantity * 0.9m);
    }

    [Fact(DisplayName = "Given a sale product with 10 to 20 items When calculating total price Then 20% discount is applied")]
    public void Given_10To20Items_When_CalculatingTotalPrice_Then_20PercentDiscount()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };

        var sale = new Sale(
            saleNumber: _faker.Random.String2(10),
            branch: new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active },
            customer: new User { Id = Guid.NewGuid(), Role = UserRole.Customer }
        );

        var saleProduct = new SaleProduct(sale, product, _faker.Random.Int(10, 20));

        // Act
        var totalPrice = saleProduct.TotalAmount;

        // Assert
        totalPrice.Should().Be(product.UnitPrice * saleProduct.Quantity * 0.8m);
    }

    [Fact(DisplayName = "Given a sale product with more than 20 items When creating Then an exception is thrown")]
    public void Given_MoreThan20Items_When_Creating_Then_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UnitPrice = _faker.Random.Decimal(10, 100)
        };

        var sale = new Sale(
            saleNumber: _faker.Random.String2(10),
            branch: new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active },
            customer: new User { Id = Guid.NewGuid(), Role = UserRole.Customer }
        );

        Action act = () => new SaleProduct(sale, product, _faker.Random.Int(21, 999));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 identical items.");
    }
}