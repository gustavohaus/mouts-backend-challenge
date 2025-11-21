using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleBusinessRulesTests
{
    private readonly Faker _faker;

    public SaleBusinessRulesTests()
    {
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a valid sale When calculating total amount Then returns correct value")]
    public void Given_ValidSale_When_CalculatingTotalAmount_Then_ReturnsCorrectValue()
    {
        // Arrange
        var sale = GenerateValidSale();

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        totalAmount.Should().Be(sale.SaleProducts.Sum(p => p.TotalAmount));
    }

    [Fact(DisplayName = "Given a sale When adding a product Then product is added successfully")]
    public void Given_Sale_When_AddingProduct_Then_ProductIsAddedSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var product = GenerateValidProduct();
        var saleProduct = new SaleProduct(sale, product, _faker.Random.Int(1, 10));

        // Act
        sale.AddProduct(saleProduct);

        // Assert
        sale.SaleProducts.Should().Contain(saleProduct);
        sale.TotalAmount.Should().Be(sale.SaleProducts.Sum(p => p.TotalAmount));
    }

    [Fact(DisplayName = "Given a sale When removing a product Then product is removed successfully")]
    public void Given_Sale_When_RemovingProduct_Then_ProductIsRemovedSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var productToRemove = sale.SaleProducts.First();

        // Act
        sale.RemoveProduct(productToRemove.ProductId);

        // Assert
        sale.SaleProducts.Should().NotContain(productToRemove);
        sale.TotalAmount.Should().Be(sale.SaleProducts.Sum(p => p.TotalAmount));
    }

    [Fact(DisplayName = "Given a sale When cancelling a product Then product is marked as cancelled")]
    public void Given_Sale_When_CancellingProduct_Then_ProductIsMarkedAsCancelled()
    {
        // Arrange
        var sale = GenerateValidSale();
        var productToCancel = sale.SaleProducts.First();

        // Act
        sale.CancelProduct(productToCancel.ProductId);

        // Assert
        productToCancel.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(sale.SaleProducts.Where(p => !p.IsCancelled).Sum(p => p.TotalAmount));
    }

    [Fact(DisplayName = "Given a sale When cancelling all products Then sale is marked as cancelled")]
    public void Given_Sale_When_CancellingAllProducts_Then_SaleIsMarkedAsCancelled()
    {
        // Arrange
        var sale = GenerateValidSale();

        // Act
        sale.Cancel();

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.SaleProducts.All(p => p.IsCancelled).Should().BeTrue();
        sale.TotalAmount.Should().Be(0);
    }

    [Fact(DisplayName = "Given a sale When adding a product with more than 20 items Then throws InvalidOperationException")]
    public void Given_Sale_When_AddingProductWithMoreThan20Items_Then_ThrowsInvalidOperationException()
    {
        // Arrange
        var sale = GenerateValidSale();
        var product = GenerateValidProduct();

        // Act
        Action act = () => sale.AddProduct(new SaleProduct(sale, product, 21));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 identical items.");
    }

    [Fact(DisplayName = "Given a sale When removing a non-existent product Then throws InvalidOperationException")]
    public void Given_Sale_When_RemovingNonExistentProduct_Then_ThrowsInvalidOperationException()
    {
        // Arrange
        var sale = GenerateValidSale();
        var nonExistentProductId = Guid.NewGuid();

        // Act
        Action act = () => sale.RemoveProduct(nonExistentProductId);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Product with ID {nonExistentProductId} not found in the sale.");
    }

    [Fact(DisplayName = "Given a sale When cancelling a non-existent product Then throws InvalidOperationException")]
    public void Given_Sale_When_CancellingNonExistentProduct_Then_ThrowsInvalidOperationException()
    {
        // Arrange
        var sale = GenerateValidSale();
        var nonExistentProductId = Guid.NewGuid();

        // Act
        Action act = () => sale.CancelProduct(nonExistentProductId);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Product with ID {nonExistentProductId} not found in the sale.");
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