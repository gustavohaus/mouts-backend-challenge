using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Represents the request for retrieving paged sales
/// </summary>
public class GetSalesRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public SaleStatus? Status { get; set; }
}