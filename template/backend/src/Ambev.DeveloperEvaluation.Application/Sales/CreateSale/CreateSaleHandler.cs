using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSaleHandler> _logger;

        public CreateSaleHandler(ISaleRepository saleRepository, IUserRepository userRepository, IBranchRepository branchRepository, IProductRepository productRepository, IMapper mapper, ILogger<CreateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _logger = logger;

        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting CreateSale operation for SaleNumber: {SaleNumber}", command.SaleNumber);

            var validator = new CreateSaleValidator();

            var customer = await _userRepository.GetByIdAsync(command.CustomerId);

            if (customer == null || customer.Role != UserRole.Customer)
            {
                _logger.LogWarning("Customer {CustomerId} does not exist or is not a valid customer.", command.CustomerId);
                throw new InvalidOperationException($"Customer {command.CustomerId} does not exist.");
            }
            var branch = await _branchRepository.GetByIdAsync(command.BranchId);

            if (branch == null || branch.Status != BranchStatus.Active)
            {
                _logger.LogWarning("Branch {BranchId} does not exist or is not active.", command.BranchId);
                throw new InvalidOperationException($"Branch {command.BranchId} does not exist.");
            }

            var sale = new Sale(command.SaleNumber, branch, customer);

            foreach (var saleProductCommand in command.Products)
            { 
                var product = await _productRepository.GetByIdAsync(saleProductCommand.ProductId);

                if(product == null)
                    throw new InvalidOperationException($"product {saleProductCommand.ProductId} not exists");

                var saleProduct = new SaleProduct(sale, product, saleProductCommand.Quantity);
                sale.AddProduct(saleProduct);
            }

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            return _mapper.Map<CreateSaleResult>(createdSale);
        }
    }
}
