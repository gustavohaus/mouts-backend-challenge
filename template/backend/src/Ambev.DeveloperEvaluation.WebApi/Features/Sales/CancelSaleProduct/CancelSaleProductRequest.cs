namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleProduct
{
    public class CancelSaleProductRequest
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
    }
}
