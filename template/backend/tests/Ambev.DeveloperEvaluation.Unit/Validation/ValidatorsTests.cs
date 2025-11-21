using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Validation;

public class ValidatorsTests
{
    [Fact(DisplayName = "CreateSaleCommandValidator: Valid data should pass validation")]
    public void CreateSaleCommandValidator_ValidData_ShouldPassValidation()
    {
        // Arrange
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand
        {
            SaleNumber = "12345",
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "CreateSaleCommandValidator: Missing SaleNumber should fail validation")]
    public void CreateSaleCommandValidator_MissingSaleNumber_ShouldFailValidation()
    {
        // Arrange
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand
        {
            SaleNumber = null,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.SaleNumber)
            .WithErrorMessage("SaleNumber is required.");
    }

    [Fact(DisplayName = "CreateSaleCommandValidator: Missing BranchId should fail validation")]
    public void CreateSaleCommandValidator_MissingBranchId_ShouldFailValidation()
    {
        // Arrange
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand
        {
            SaleNumber = "TesteVenda",
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.BranchId)
            .WithErrorMessage("Branch ID is required.");
    }

    [Fact(DisplayName = "CreateSaleCommandValidator:Invalid quantity under")]
    public void CreateSaleCommandValidator_InvalidQuantity_ShouldFailValidation()
    {
        // Arrange
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand
        {
            BranchId = Guid.NewGuid(),
            SaleNumber = "TesteVenda",
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = -1 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

          // Assert
        result.ShouldHaveAnyValidationError();
    }
    [Fact(DisplayName = "CreateSaleCommandValidator:Invalid quantity over 20")]
    public void CreateSaleCommandValidator_InvalidQuantityOver_ShouldFailValidation()
    {
        // Arrange
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand
        {
            BranchId = Guid.NewGuid(),
            SaleNumber = "TesteVenda",
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 21 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Fact(DisplayName = "CreateSaleCommandValidator: Missing CustomerId should fail validation")]
    public void CreateSaleCommandValidator_MissingCustomerId_ShouldFailValidation()
    {
        // Arrange
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand
        {
            SaleNumber = "TesteVenda",
            BranchId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CustomerId)
            .WithErrorMessage("Customer ID is required.");
    }

    [Fact(DisplayName = "DeleteSaleCommandValidator: Valid SaleId should pass validation")]
    public void DeleteSaleCommandValidator_ValidSaleId_ShouldPassValidation()
    {
        // Arrange
        var validator = new DeleteSaleCommandValidator();
        var command = new DeleteSaleCommand { SaleId = Guid.NewGuid() };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "DeleteSaleCommandValidator: Empty SaleId should fail validation")]
    public void DeleteSaleCommandValidator_EmptySaleId_ShouldFailValidation()
    {
        // Arrange
        var validator = new DeleteSaleCommandValidator();
        var command = new DeleteSaleCommand ();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.SaleId)
            .WithErrorMessage("Sale ID is required.");
    }

    [Fact(DisplayName = "UpdateSaleCommandValidator: Valid data should pass validation")]
    public void UpdateSaleCommandValidator_ValidData_ShouldPassValidation()
    {
        // Arrange
        var validator = new UpdateSaleCommandValidator();
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleProducts = new List<UpdateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 5 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "UpdateSaleCommandValidator: Empty SaleId should fail validation")]
    public void UpdateSaleCommandValidator_EmptySaleId_ShouldFailValidation()
    {
        // Arrange
        var validator = new UpdateSaleCommandValidator();
        var command = new UpdateSaleCommand
        {
            Id = Guid.Empty,
            SaleProducts = new List<UpdateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 5 }
            }
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Sale ID is required.");
    }
}