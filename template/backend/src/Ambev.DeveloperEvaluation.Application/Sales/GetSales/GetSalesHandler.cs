using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, PagedSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<PagedSalesResult> Handle(GetSalesCommand request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetPagedSalesAsync(
            request.PageNumber,
            request.PageSize,
            request.StartDate,
            request.EndDate,
            request.CustomerId,
            request.BranchId,
            request.Status,
            cancellationToken
        );

        return _mapper.Map<PagedSalesResult>(sales);
    }
}