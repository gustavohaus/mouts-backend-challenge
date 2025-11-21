
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


        protected SaleProduct()
        {

        }
        public SaleProduct(Sale sale, Product product, int quantity)
        {
            Sale = sale;
            SaleId = sale.Id;
            Product = product;
            ProductId = product.Id;
            Quantity = quantity;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            this.ApplyDiscountIfEligible();
            this.UpdateTotalAmount();
        }

        public void Update(int quantity)
        {
            this.Quantity = quantity;

            this.ApplyDiscountIfEligible();
            this.UpdateTotalAmount();
        }
        public void Cancel()
        {
            IsCancelled = true;

        }

        private void ApplyDiscountIfEligible()
        {
            if (Quantity >= 4 && Quantity < 10)
                DiscountPercent = 10.0m;
            else if (Quantity >= 10 && Quantity <= 20)
                DiscountPercent = 20.0m;
        }


        private void UpdateTotalAmount()
        {
            var baseAmount = Product.UnitPrice * Quantity;
            TotalAmount = baseAmount - (baseAmount * (DiscountPercent / 100));
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
