using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ProductRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly ApiDBContext _dbContext;
        private readonly ProductRepository _productRepository;
        public ProductRepositoryIntegrationTests(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _productRepository = new ProductRepository(_dbContext);
        }
        [Fact]
        public async Task GetProducts_WhenProductsExist_ReturnsAllProductsWithCategory()
        {
           
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();

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
            // Arrange
            _dbContext.Products.RemoveRange(_dbContext.Products);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

    }
}
