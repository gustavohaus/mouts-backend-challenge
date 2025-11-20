
namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleProduct
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public Sale Sale { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; private set; }
        public decimal DiscountPercent { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
