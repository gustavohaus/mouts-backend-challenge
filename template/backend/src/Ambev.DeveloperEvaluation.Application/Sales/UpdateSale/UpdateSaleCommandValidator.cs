using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand
/// </summary>
public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleCommand
    /// </summary>
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required.");

        RuleFor(x => x.SaleProducts)
            .NotEmpty()
            .WithMessage("At least one product is required.")
            .Must(products => products.All(p => p.Quantity > 0))
            .WithMessage("Product quantities must be greater than zero.");

        RuleForEach(x => x.SaleProducts).ChildRules(product =>
        {
            product.RuleFor(p => p.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required.");

            product.RuleFor(p => p.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20)
                .WithMessage("Quantity cannot exceed 20.");
        });
    }
}