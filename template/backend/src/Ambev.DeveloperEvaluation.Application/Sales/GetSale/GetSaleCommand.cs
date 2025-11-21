using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for retrieving paged sales
/// </summary>
public class GetSaleCommand : IRequest<GetSaleResult>
{
    public Guid SaleId { get; set; }

    public GetSaleCommand(Guid saleId)
    {
        SaleId = saleId;
    }
}