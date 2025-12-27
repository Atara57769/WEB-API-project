using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ProductRepositoryIntegrationTests : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly ApiDBContext _dbContext;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _productRepository = new ProductRepository(_dbContext);
        }
        public void Dispose()
        {
            _fixture.Dispose();
        }

        [Fact]
        public async Task GetProducts_WhenProductsExist_ReturnsAllProductsWithCategory()
        {
            // Arrange
            var category = new Category { Name = "Electronics" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var testProducts = new List<Product>
            {
                new Product { ProductName = "Laptop", CategoryId = category.Id, Price = 3500 },
                new Product { ProductName = "Mouse", CategoryId = category.Id, Price = 150 }
            };

            await _dbContext.Products.AddRangeAsync(testProducts);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.NotNull(p.Category));
            Assert.Contains(result, p => p.ProductName == "Laptop" && p.Category.Name == "Electronics");
        }

        [Fact]
        public async Task GetProducts_WhenNoProductsExist_ReturnsEmptyList()
        {
            // Act
            var result = await _productRepository.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}