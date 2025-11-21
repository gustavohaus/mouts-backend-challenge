using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Represents a paged result of sales
/// </summary>
public class GetSaleResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public SaleStatus Status { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public SaleUserDto Customer { get; set; }
    public BranchDto Branch { get; set; }
    public List<SaleProductsDto> SaleProducts { get; set; }
}

public class SaleUserDto
{
    public Guid Id { get; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
public class SaleProductsDto
{
    public Guid Id { get; set; }
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal UnitPrice { get; set; }
}
public class BranchDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
}
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
}