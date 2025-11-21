using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{

    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IValidator<DeleteSaleCommand> _validator;
        private readonly ILogger<DeleteSaleHandler> _logger;

        public DeleteSaleHandler(ISaleRepository saleRepository, IValidator<DeleteSaleCommand> validator, ILogger<DeleteSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale with ID {SaleId} not found.", request.SaleId);
                throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(request.SaleId), $"Sale with ID {request.SaleId} not found.") });
            }

            await _saleRepository.DeleteAsync(sale, cancellationToken);
            return true;
        }
    }
}
