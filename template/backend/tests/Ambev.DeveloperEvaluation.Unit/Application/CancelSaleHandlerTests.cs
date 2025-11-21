using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.NetworkInformation;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;
using ValidationException = FluentValidation.ValidationException;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IValidator<CancelSaleCommand> _validator;
    private readonly CancelSaleHandler _handler;
    private readonly Faker _faker;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _validator = Substitute.For<IValidator<CancelSaleCommand>>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _logger, _validator);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given a valid sale ID When cancelling sale Then cancels successfully")]
    public async Task Handle_ValidSaleId_CancelsSuccessfully()
    {
        // Arrange
        var sale = GenerateValidSale();
        var command = new CancelSaleCommand { SaleId = sale.Id };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        sale.Status.Should().Be(SaleStatus.Cancelled);
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given a non-existent sale ID When cancelling sale Then throws ValidationException")]
    public async Task Handle_NonExistentSaleId_ThrowsValidationException()
    {
        // Arrange
        var command = new CancelSaleCommand { SaleId = Guid.NewGuid() };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }


    private Sale GenerateValidSale()
    {
        var branch = new Branch { Id = Guid.NewGuid(), Status = BranchStatus.Active };
        var customer = new User { Id = Guid.NewGuid(), Role = UserRole.Customer };
        return new Sale(_faker.Random.String2(10), branch, customer);
    }
}