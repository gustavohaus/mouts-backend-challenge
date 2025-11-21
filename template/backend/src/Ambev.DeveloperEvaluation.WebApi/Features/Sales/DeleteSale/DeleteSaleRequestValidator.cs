using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
    {
        public DeleteSaleRequestValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
