using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{

    public class UpdateSaleCommand : IRequest<GetSaleResult>
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<UpdateSaleProductsCommand> SaleProducts { get; set; }
    }

    public class UpdateSaleProductsCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
