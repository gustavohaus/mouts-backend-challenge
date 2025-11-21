using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleProduct
{

    public class CancelSaleProductCommand : IRequest<bool>
    {
        public Guid SaleId { get; set; }    
        public Guid ProductId { get; set; }    
    }

}
