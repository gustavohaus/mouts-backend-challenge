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
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
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

        public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CancelSaleCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

            if (sale == null)
            {
                _logger.LogWarning("Sale {Id} not found.", command.SaleId);
                throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");
            }

            sale.Cancel();

            await _saleRepository.UpdateAsync(sale, cancellationToken);
            return _mapper.Map<CancelSaleResult>(sale);
        }
    }
}
