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
    public class DeleteSaleCommand : IRequest<bool>
    {
        public Guid SaleId { get; set; }    
    }

}
