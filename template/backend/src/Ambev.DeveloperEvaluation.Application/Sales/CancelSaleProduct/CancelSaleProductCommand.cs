using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{

    /// <summary>
    /// Command to create a new sale
    /// </summary>
    public class CancelSaleProductCommand : IRequest<CancelSaleProductResult>
    {
        public Guid SaleId { get; set; }    
        public Guid SaleProduct { get; set; }    
    }

}
