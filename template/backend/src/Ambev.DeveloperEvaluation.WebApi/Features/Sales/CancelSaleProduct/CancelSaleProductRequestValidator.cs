using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleProduct
{
    public class CancelSaleProductRequestValidator : AbstractValidator<CancelSaleProductRequest>
    {
        public CancelSaleProductRequestValidator()
        {
            RuleFor(x => x.SaleId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
