using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{

    /// <summary>
    /// Command to create a new sale
    /// </summary>
    /// 
    [ExcludeFromCodeCoverage]
    public class UpdateSaleCommand : IRequest<GetSaleResult>
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<UpdateSaleProductsCommand> SaleProducts { get; set; }
    }

    /// <summary>
    /// Represents a product in the sale result
    /// </summary>
    public class UpdateSaleProductsCommand
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SaleId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
    }

}
