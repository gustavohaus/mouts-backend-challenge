using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public SaleStatus Status { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleUserResponse Customer { get; set; }
        public BranchResponse Branch { get; set; }
        public List<SaleProductsResponse> SaleProducts { get; set; }
    }

    public class SaleUserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class SaleProductsResponse
    {
        public Guid Id { get; set; }
        public ProductResponse Product { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class BranchResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

