using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SaleDate).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleForEach(x => x.SaleProducts).SetValidator(new UpdateSaleProductRequestValidator());
        }
    }

    public class UpdateSaleProductRequestValidator : AbstractValidator<UpdateSaleProductRequest>
    {
        public UpdateSaleProductRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.Quantity).LessThanOrEqualTo(20);
        }
    }
}
