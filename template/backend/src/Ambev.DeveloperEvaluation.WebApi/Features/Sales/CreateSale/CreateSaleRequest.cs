using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{

    public class CreateSaleRequest 
    {
        public string SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<CreateSaleProductRequest> Products { get; set; }
    }

    public class CreateSaleProductRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
