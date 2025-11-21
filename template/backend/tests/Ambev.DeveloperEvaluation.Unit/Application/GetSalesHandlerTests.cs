using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSalesHandler _handler;
    private readonly Faker _faker;

    public GetSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSalesHandler(_saleRepository, _mapper);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given all filters When retrieving sales Then returns filtered sales")]
    public async Task Handle_AllFilters_ReturnsFilteredSales()
    {
        // Arrange
        var command = new GetSalesCommand
        {
            PageNumber = 1,
            PageSize = 10,
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Status = SaleStatus.Created
        };

        var sales = GenerateSales(10);
        _saleRepository.GetPagedSalesAsync(
            command.PageNumber,
            command.PageSize,
            command.StartDate,
            command.EndDate,
            command.CustomerId,
            command.BranchId,
            command.Status,
            Arg.Any<CancellationToken>()
        ).Returns((sales, sales.Count));

        var expectedResult = new PagedSalesResult
        {
            TotalCount = sales.Count,
            PageNumber = command.PageNumber,
            PageSize = command.PageSize,
            Sales = sales.Select(s => new GetSaleResult { SaleNumber = s.SaleNumber }).ToList()
        };

        _mapper.Map<PagedSalesResult>(Arg.Any<(List<Sale>, int)>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(sales.Count);
        result.Sales.Should().HaveCount(10);
        await _saleRepository.Received(1).GetPagedSalesAsync(
            command.PageNumber,
            command.PageSize,
            command.StartDate,
            command.EndDate,
            command.CustomerId,
            command.BranchId,
            command.Status,
            Arg.Any<CancellationToken>()
        );
    }

    [Fact(DisplayName = "Given no filters When retrieving sales Then returns all sales")]
    public async Task Handle_NoFilters_ReturnsAllSales()
    {
        // Arrange
        var command = new GetSalesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var sales = GenerateSales(10);
        _saleRepository.GetPagedSalesAsync(
            command.PageNumber,
            command.PageSize,
            null,
            null,
            null,
            null,
            null,
            Arg.Any<CancellationToken>()
        ).Returns((sales, sales.Count));

        var expectedResult = new PagedSalesResult
        {
            TotalCount = sales.Count,
            PageNumber = command.PageNumber,
            PageSize = command.PageSize,
            Sales = sales.Select(s => new GetSaleResult { SaleNumber = s.SaleNumber }).ToList()
        };

        _mapper.Map<PagedSalesResult>(Arg.Any<(List<Sale>, int)>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(sales.Count);
        result.Sales.Should().HaveCount(10);
        await _saleRepository.Received(1).GetPagedSalesAsync(
            command.PageNumber,
            command.PageSize,
            null,
            null,
            null,
            null,
            null,
            Arg.Any<CancellationToken>()
        );
    }

    private List<Sale> GenerateSales(int count)
    {
        return new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                f.Random.String2(10),
                new Branch { Id = f.Random.Guid(), Status = BranchStatus.Active },
                new User { Id = f.Random.Guid(), Role = UserRole.Customer }
            ))
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past())
            .RuleFor(s => s.Status, f => f.PickRandom<SaleStatus>())
            .Generate(count);
    }
}