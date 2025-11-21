//using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.Domain.Enums;
//using Ambev.DeveloperEvaluation.ORM;
//using Ambev.DeveloperEvaluation.ORM.Repositories;
//using Bogus;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using NSubstitute;
//using Xunit;

//namespace Ambev.DeveloperEvaluation.Unit.Repository;

//public class BranchRepositoryTests
//{
//    private readonly DefaultContext _dbContext;
//    private readonly BranchRepository _repository;
//    private readonly Faker _faker;

//    public BranchRepositoryTests()
//    {
//        _dbContext = Substitute.For<DefaultContext>();
//        _repository = new BranchRepository(_dbContext);
//        _faker = new Faker();
//    }

//    [Fact(DisplayName = "Given a valid branch ID When retrieving branch Then it should return the correct branch")]
//    public async Task GetBranchById_ValidId_ShouldReturnCorrectBranch()
//    {
//        // Arrange
//        var branch = GenerateValidBranch();
//        _dbContext.Set<Branch>().FindAsync(branch.Id).Returns(branch);

//        // Act
//        var result = await _repository.GetByIdAsync(branch.Id);

//        // Assert
//        result.Should().NotBeNull();
//        result.Id.Should().Be(branch.Id);
//        await _dbContext.Set<Branch>().Received(1).FindAsync(branch.Id);
//    }

//    private Branch GenerateValidBranch()
//    {
//        return new Branch
//        {
//            Id = _faker.Random.Guid(),
//            Status = BranchStatus.Active,
//        };
//    }
//}