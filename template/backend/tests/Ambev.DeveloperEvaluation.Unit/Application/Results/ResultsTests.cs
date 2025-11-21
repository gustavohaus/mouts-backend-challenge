using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Results;

public class ResultsTests
{
    [Fact(DisplayName = "CancelSaleResult: Properties should be set correctly")]
    public void CancelSaleResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = new CancelSaleResult { Id = id };

        // Assert
        result.Id.Should().Be(id);
    }

    [Fact(DisplayName = "CreateSaleResult: Properties should be set correctly")]
    public void CreateSaleResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var saleNumber = "12345";
        var saleDate = DateTime.UtcNow;
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var totalAmount = 100.50m;
        var saleProducts = new List<CreateSaleProductResult>
        {
            new CreateSaleProductResult
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 50.25m,
                Discount = 10,
                TotalAmount = 90,
                IsCancelled = false
            }
        };

        // Act
        var result = new CreateSaleResult
        {
            Id = Guid.NewGuid(),
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

    [Fact(DisplayName = "CreateSaleProductResult: Properties should be set correctly")]
    public void CreateSaleProductResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 2;
        var unitPrice = 50.25m;
        var discount = 10;
        var totalAmount = 90;
        var isCancelled = false;

        // Act
        var result = new CreateSaleProductResult
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            TotalAmount = totalAmount,
            IsCancelled = isCancelled
        };

        // Assert
        result.ProductId.Should().Be(productId);
        result.Quantity.Should().Be(quantity);
        result.UnitPrice.Should().Be(unitPrice);
        result.Discount.Should().Be(discount);
        result.TotalAmount.Should().Be(totalAmount);
        result.IsCancelled.Should().Be(isCancelled);
    }


    [Fact(DisplayName = "GetSaleResult: Properties should be set correctly")]
    public void GetSaleResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var saleNumber = "12345";
        var saleDate = DateTime.UtcNow;
        var customer = new SaleUserDto { Id = Guid.NewGuid(), Username = "Customer" };
        var branch = new BranchDto { Id = Guid.NewGuid(), Name = "Branch" };
        var products = new List<SaleProductsDto>
        {
            new SaleProductsDto
            {
                Quantity = 2,
                UnitPrice = 50.25m,
                DiscountPercent = 10
            }
        };

        // Act
        var result = new GetSaleResult
        {
            Id = id,
            SaleNumber = saleNumber,
            SaleDate = saleDate,
            Customer = customer,
            Branch = branch,
            SaleProducts = products
        };

        // Assert
        result.Id.Should().Be(id);
        result.SaleNumber.Should().Be(saleNumber);
        result.SaleDate.Should().Be(saleDate);
        result.Customer.Should().Be(customer);
        result.Branch.Should().Be(branch);
        result.SaleProducts.Should().BeEquivalentTo(products);
    }

    [Fact(DisplayName = "UpdateSaleResult: Properties should be set correctly")]
    public void UpdateSaleResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var saleNumber = "12345";
        var saleDate = DateTime.UtcNow;
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var totalAmount = 100.50m;
        var products = new List<UpdateSaleProductsResult>
        {
            new UpdateSaleProductsResult
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 50.25m,
                Discount = 10,
                TotalAmount = 90,
                IsCancelled = false
            }
        };

        // Act
        var result = new UpdateSaleResult
        {
            Id = id,
            SaleNumber = saleNumber,
            SaleDate = saleDate,
            CustomerId = customerId,
            BranchId = branchId,
            TotalAmount = totalAmount,
            Products = products
        };

        // Assert
        result.Id.Should().Be(id);
        result.SaleNumber.Should().Be(saleNumber);
        result.SaleDate.Should().Be(saleDate);
        result.CustomerId.Should().Be(customerId);
        result.BranchId.Should().Be(branchId);
        result.TotalAmount.Should().Be(totalAmount);
        result.Products.Should().BeEquivalentTo(products);
    }

    [Fact(DisplayName = "UpdateSaleProductsResult: Properties should be set correctly")]
    public void UpdateSaleProductsResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 2;
        var unitPrice = 50.25m;
        var discount = 10;
        var totalAmount = 90;
        var isCancelled = false;

        // Act
        var result = new UpdateSaleProductsResult
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            TotalAmount = totalAmount,
            IsCancelled = isCancelled
        };

        // Assert
        result.ProductId.Should().Be(productId);
        result.Quantity.Should().Be(quantity);
        result.UnitPrice.Should().Be(unitPrice);
        result.Discount.Should().Be(discount);
        result.TotalAmount.Should().Be(totalAmount);
        result.IsCancelled.Should().Be(isCancelled);
    }
}