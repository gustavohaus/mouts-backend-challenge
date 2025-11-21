using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; }
        public string Description{ get; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();
     
    }
}
