//using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.ORM;
//using Ambev.DeveloperEvaluation.ORM.Repositories;
//using Bogus;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using NSubstitute;
//using Xunit;

//namespace Ambev.DeveloperEvaluation.Unit.Repository;

//public class ProductRepositoryTests
//{
//    private readonly DefaultContext _dbContext;
//    private readonly ProductRepository _repository;
//    private readonly Faker _faker;

//    public ProductRepositoryTests()
//    {
//        _dbContext = Substitute.For<DefaultContext>();
//        _repository = new ProductRepository(_dbContext);
//        _faker = new Faker();
//    }


//    [Fact(DisplayName = "Given a valid product ID When retrieving product Then it should return the correct product")]
//    public async Task GetProductById_ValidId_ShouldReturnCorrectProduct()
//    {
//        // Arrange
//        var product = GenerateValidProduct();
//        _dbContext.Products.Add(product);
//        await _dbContext.SaveChangesAsync();

//        // Act
//        var result = await _repository.GetByIdAsync(product.Id);

//        // Assert
//        result.Should().NotBeNull();
//        result.Id.Should().Be(product.Id);
//    }

//    private Product GenerateValidProduct()
//    {
//        return new Product
//        {
//            Id = _faker.Random.Guid(),
//            Name = _faker.Commerce.ProductName(),
//            Description = _faker.Commerce.ProductDescription(),
//            UnitPrice = _faker.Random.Decimal(10, 100),
//        };
//    }
//}