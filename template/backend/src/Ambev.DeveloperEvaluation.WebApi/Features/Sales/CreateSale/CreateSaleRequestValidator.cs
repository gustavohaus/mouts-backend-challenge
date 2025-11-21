using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number must be at most 50 characters long.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");
            RuleForEach(x => x.Products).SetValidator(new SaleProductRequestValidator());
        }
    }

    /// <summary>
    /// Validator for SaleProductRequest
    /// </summary>
    public class SaleProductRequestValidator : AbstractValidator<CreateSaleProductRequest>
    {
        public SaleProductRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 units.");
        }
    }
}
