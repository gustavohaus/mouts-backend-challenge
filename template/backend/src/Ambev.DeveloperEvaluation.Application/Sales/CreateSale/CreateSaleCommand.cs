using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{

    /// <summary>
    /// Command to create a new sale
    /// </summary>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<CreateSaleProductsCommand> Products { get; set; }
    }

    /// <summary>
    /// Represents a product in the sale result
    /// </summary>
    public class CreateSaleProductsCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
     
    }

}
