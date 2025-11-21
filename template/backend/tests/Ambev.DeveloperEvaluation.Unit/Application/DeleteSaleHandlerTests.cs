using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;
using ValidationException = FluentValidation.ValidationException;
using NSubstitute.ReturnsExtensions;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IValidator<DeleteSaleCommand> _validator;
    private readonly ILogger<DeleteSaleHandler> _logger;
    private readonly DeleteSaleHandler _handler;
    private readonly Faker _faker;

    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _validator = Substitute.For<IValidator<DeleteSaleCommand>>();
        _logger = Substitute.For<ILogger<DeleteSaleHandler>>();
        _handler = new DeleteSaleHandler(_saleRepository, _validator, _logger);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a valid sale ID When deleting sale Then deletes successfully")]
    public async Task Handle_ValidSaleId_DeletesSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var command = new DeleteSaleCommand { SaleId = sale.Id };

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.DeleteAsync(sale, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _saleRepository.Received(1).DeleteAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given a non-existent sale ID When deleting sale Then throws ValidationException")]
    public async Task Handle_NonExistentSaleId_ThrowsValidationException()
    {
        // Arrange
        var command = new DeleteSaleCommand { SaleId = Guid.NewGuid() };

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    private Sale GenerateValidSale()
    {
        var branch = new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active };
        var customer = new User { Id = Guid.NewGuid(), Role = UserRole.Customer };
        return new Sale(_faker.Random.String2(10), branch, customer);
    }
}