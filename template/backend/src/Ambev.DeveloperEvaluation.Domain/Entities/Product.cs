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
        public string Name { get; set; } = string.Empty;
        public string Description{ get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; } = DateTime.UtcNow;
        public ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();


        public Product()
        {
            
        }

    }
}
