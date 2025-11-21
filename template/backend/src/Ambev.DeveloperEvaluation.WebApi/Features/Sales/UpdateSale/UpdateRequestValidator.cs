using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number must be at most 50 characters long.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleForEach(x => x.SaleProducts).SetValidator(new UpdateSaleProductRequestValidator());

            RuleFor(x => x.SaleProducts)
                       .NotEmpty().WithMessage("Products list cannot be empty.")
                       .Must(products =>
                       {
                           var productIds = products.Select(p => p.ProductId).ToList();
                           return productIds.Distinct().Count() == productIds.Count;
                       })
                       .WithMessage("Duplicate products are not allowed in the Products list.");
        }
    }

    public class UpdateSaleProductRequestValidator : AbstractValidator<UpdateSaleProductRequest>
    {
        public UpdateSaleProductRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 units.");

        }
    }
}
