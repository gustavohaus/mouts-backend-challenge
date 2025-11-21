using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IValidator<CreateSaleCommand> _validator;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _validator = Substitute.For<IValidator<CreateSaleCommand>>();
        _handler = new CreateSaleHandler(
            _saleRepository,
            _userRepository,
            _branchRepository,
            _productRepository,
            _mapper,
            _logger,
            _validator
        );
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "12345",
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Products = new List<CreateSaleProductsCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        var customer = new User { Id = command.CustomerId, Role = UserRole.Customer };
        var branch = new Branch { Id = command.BranchId, Status = BranchStatus.Active };
        var product = new Product { Id = command.Products.First().ProductId };
        var sale = new Sale(command.SaleNumber, branch, customer);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.GetByIdAsync(command.Products.First().ProductId, Arg.Any<CancellationToken>()).Returns(product);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(new CreateSaleResult { SaleNumber = sale.SaleNumber });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new("SaleNumber", "SaleNumber is required")
            }));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given non-existent customer When creating sale Then throws invalid operation exception")]
    public async Task Handle_NonExistentCustomer_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new CreateSaleCommand { CustomerId = Guid.NewGuid() };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns((User)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact(DisplayName = "Given non-existent branch When creating sale Then throws invalid operation exception")]
    public async Task Handle_NonExistentBranch_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new CreateSaleCommand { BranchId = Guid.NewGuid() };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(new User { Role = UserRole.Customer });
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>()).Returns((Branch)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact(DisplayName = "Given non-existent product When creating sale Then throws invalid operation exception")]
    public async Task Handle_NonExistentProduct_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Products = new List<CreateSaleProductsCommand> { new() { ProductId = Guid.NewGuid() } }
        };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(new User { Role = UserRole.Customer });
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>()).Returns(new Branch { Status = BranchStatus.Active });
        _productRepository.GetByIdAsync(command.Products.First().ProductId, Arg.Any<CancellationToken>()).Returns((Product)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}