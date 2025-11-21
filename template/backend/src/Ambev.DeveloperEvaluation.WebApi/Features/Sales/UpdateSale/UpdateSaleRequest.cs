using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{

    public class UpdateSaleRequest
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<UpdateSaleProductRequest> SaleProducts { get; set; }
    }

    public class UpdateSaleProductRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
