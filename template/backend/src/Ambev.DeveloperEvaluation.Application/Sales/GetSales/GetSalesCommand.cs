using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Command for retrieving paged sales
/// </summary>
public class GetSalesCommand : IRequest<PagedSalesResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public SaleStatus? Status { get; set; }
}