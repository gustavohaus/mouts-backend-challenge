//using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.Domain.Enums;
//using Ambev.DeveloperEvaluation.ORM;
//using Ambev.DeveloperEvaluation.ORM.Repositories;
//using Bogus;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using NSubstitute;
//using Xunit;

//namespace Ambev.DeveloperEvaluation.Unit.Domain.Repositories;

//public class SaleRepositoryTests
//{
//    private readonly DefaultContext _dbContext;
//    private readonly SaleRepository _repository;
//    private readonly Faker _faker;

//    public SaleRepositoryTests()
//    {
//        _dbContext = Substitute.ForPartsOf<DefaultContext>(new DbContextOptions<DefaultContext>());
//        _repository = new SaleRepository(_dbContext);
//        _faker = new Faker();
//    }

//    [Fact(DisplayName = "Given a filter When retrieving sales Then it should return filtered results")]
//    public async Task GetSales_WithFilter_ShouldReturnFilteredResults()
//    {
//        // Arrange
//        var sales = GenerateSales(10).AsQueryable();
//        var mockDbSet = CreateMockDbSet(sales);

//        _dbContext.Set<Sale>().Returns(mockDbSet);

//        var pageNumber = 1;
//        var pageSize = 5;
//        var startDate = DateTime.UtcNow.AddDays(-30);
//        var endDate = DateTime.UtcNow;
//        var customerId = sales.First().CustomerId;
//        var branchId = sales.First().BranchId;
//        var status = SaleStatus.Created;

//        // Act
//        var (result, totalCount) = await _repository.GetPagedSalesAsync(
//            pageNumber,
//            pageSize,
//            startDate,
//            endDate,
//            customerId,
//            branchId,
//            status
//        );

//        // Assert
//        result.Should().NotBeNullOrEmpty();
//        result.All(s => s.Status == SaleStatus.Created).Should().BeTrue();
//        totalCount.Should().Be(sales.Count(s => s.Status == SaleStatus.Created));
//    }

//    private DbSet<T> CreateMockDbSet<T>(IQueryable<T> data) where T : class
//    {
//        var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();
//        ((IQueryable<T>)mockSet).Provider.Returns(data.Provider);
//        ((IQueryable<T>)mockSet).Expression.Returns(data.Expression);
//        ((IQueryable<T>)mockSet).ElementType.Returns(data.ElementType);
//        ((IQueryable<T>)mockSet).GetEnumerator().Returns(data.GetEnumerator());
//        return mockSet;
//    }

//    private List<Sale> GenerateSales(int count)
//    {
//        return new Faker<Sale>()
//            .CustomInstantiator(f => new Sale(
//                f.Random.String2(10),
//                new Branch { Id = f.Random.Guid(), Status = BranchStatus.Active },
//                new User { Id = f.Random.Guid(), Role = UserRole.Customer }
//            ))
//            .RuleFor(s => s.Id, f => f.Random.Guid())
//            .RuleFor(s => s.CreatedAt, f => f.Date.Past())
//            .RuleFor(s => s.Status, f => f.PickRandom<SaleStatus>())
//            .Generate(count);
//    }
//}
