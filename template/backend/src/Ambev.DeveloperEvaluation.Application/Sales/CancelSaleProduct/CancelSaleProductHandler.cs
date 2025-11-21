using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleProduct
{

    public class CancelSaleProductHandler : IRequestHandler<CancelSaleProductCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelSaleProductHandler> _logger;

        public CancelSaleProductHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CancelSaleProductHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(CancelSaleProductCommand command, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

            if (sale == null)
            {
                _logger.LogWarning("Sale {SaleId} does not exist or is not valid.", command.SaleId);
                throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(command.SaleId), $"Sale {command.SaleId} not found.") });
            }

            sale.CancelProduct(command.ProductId);

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            return true;
        }
    }
}
