using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Handler for processing CancelSaleCommand requests
    /// </summary>
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelSaleHandler> _logger;
        private readonly IValidator<CancelSaleCommand> _validator;

        public CancelSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            ILogger<CancelSaleHandler> logger,
            IValidator<CancelSaleCommand> validator)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<bool> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

            if (sale == null)
            {
                _logger.LogWarning("Sale {SaleId} does not exist or is not valid.", command.SaleId);
                throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(command.SaleId), $"Sale {command.SaleId} not found.") });
            }

            sale.Cancel();

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            return true;
        }
    }
}
