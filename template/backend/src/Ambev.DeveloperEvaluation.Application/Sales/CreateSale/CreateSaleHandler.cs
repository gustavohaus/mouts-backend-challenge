using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

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
    private readonly IValidator<CreateSaleCommand> _validator;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IUserRepository userRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger,
        IValidator<CreateSaleCommand> validator)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting CreateSale operation. SaleNumber: {SaleNumber}, CustomerId: {CustomerId}, BranchId: {BranchId}",
            command.SaleNumber,
            command.CustomerId,
            command.BranchId);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateSaleCommand: {Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _userRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer == null || customer.Role != UserRole.Customer)
        {
            _logger.LogWarning("Customer {CustomerId} does not exist or is not a valid customer.", command.CustomerId);
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(command.CustomerId), $"Customer {command.CustomerId} does not exist or is not a valid customer.") });
        }

        var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
        if (branch == null || branch.Status != BranchStatus.Active)
        {
            _logger.LogWarning("Branch {BranchId} does not exist or is not active.", command.BranchId);
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(command.BranchId), $"Branch {command.BranchId} does not exist or is not active.") });
        }

        var sale = new Sale(command.SaleNumber, branch, customer);

        foreach (var saleProductCommand in command.Products)
        {
            var product = await _productRepository.GetByIdAsync(saleProductCommand.ProductId, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} does not exist.", saleProductCommand.ProductId);
                throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(saleProductCommand.ProductId), $"Product {saleProductCommand.ProductId} does not exist.") });
            }

            // Add product to sale
            var saleProduct = new SaleProduct(sale, product, saleProductCommand.Quantity);
            sale.AddProduct(saleProduct);
        }

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        _logger.LogInformation("Sale {SaleNumber} created successfully.", command.SaleNumber);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}
