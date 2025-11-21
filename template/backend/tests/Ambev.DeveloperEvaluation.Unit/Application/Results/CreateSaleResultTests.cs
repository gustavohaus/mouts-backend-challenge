using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Results;

public class CreateSaleResultTests
{
    [Fact(DisplayName = "Given valid data When initializing CreateSaleResult Then properties are set correctly")]
    public void Initialize_CreateSaleResult_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var saleNumber = "12345";
        var saleDate = DateTime.UtcNow;
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var totalAmount = 100.50m;
        var saleProducts = new List<CreateSaleProductResult>
        {
            new CreateSaleProductResult { UnitPrice = 50.25m, Discount = 10 }
        };

        // Act
        var result = new CreateSaleResult
        {
            SaleNumber = saleNumber,
            SaleDate = saleDate,
            CustomerId = customerId,
            BranchId = branchId,
            TotalAmount = totalAmount,
            SaleProducts = saleProducts
        };

        // Assert
        result.SaleNumber.Should().Be(saleNumber);
        result.SaleDate.Should().Be(saleDate);
        result.CustomerId.Should().Be(customerId);
        result.BranchId.Should().Be(branchId);
        result.TotalAmount.Should().Be(totalAmount);
        result.SaleProducts.Should().BeEquivalentTo(saleProducts);
    }
}