using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleProduct
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
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

            sale.CancelProduct(command.SaleProduct);

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            return true;
        }
    }
}
