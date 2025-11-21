using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleHandler> _logger;

        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IProductRepository productRepository, ILogger<UpdateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<GetSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
            "Starting UpdateSale operation. SaleNumber: {SaleNumber}, CustomerId: {CustomerId}, BranchId: {BranchId}",
            command.SaleNumber,
            command.CustomerId,
            command.BranchId);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

            if (sale == null)
            {
                _logger.LogWarning("Sale {SaleId} does not exist or is not valid.", command.Id);
                throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(command.Id), $"Sale {command.Id} not found.") });
            }

            sale.UpdateInfo(command.SaleNumber);

            sale.SyncSaleProducts(command.SaleProducts.Select(p => (p.ProductId, p.Quantity)));

            var addProducts = command.SaleProducts
               .Where(e => !sale.SaleProducts.Any(p => p.ProductId == e.ProductId))
               .ToList();

            foreach (var saleProductCommand in addProducts)
            {
                var product = await _productRepository.GetByIdAsync(saleProductCommand.ProductId, cancellationToken);
                if (product == null)
                {
                    _logger.LogWarning("Product {ProductId} does not exist.", saleProductCommand.ProductId);
                    throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(saleProductCommand.ProductId), $"Product {saleProductCommand.ProductId} does not exist.") });
                }

                sale.AddProduct(new SaleProduct(sale, product, saleProductCommand.Quantity));
            }

            var updateSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

            return _mapper.Map<GetSaleResult>(updateSale);
        }
    }
}
