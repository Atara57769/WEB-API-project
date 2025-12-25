using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;

namespace TestProject
{
    public class ProductRepositoryUnitTests
    {
        [Fact]
        public async Task GetProducts_ReturnsAllProducts_WhenProductsExist()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Electronics" };
            var products = new List<Product>
            {
                new Product { Id = 1, ProductName = "Laptop", CategoryId = 1, Category = category },
                new Product { Id = 2, ProductName = "Smartphone", CategoryId = 1, Category = category }
            };

            var mockContext = new Mock<ApiDBContext>();
            mockContext
                .Setup(x => x.Products)
                .ReturnsDbSet(products);

            var productRepository = new ProductRepository(mockContext.Object);

            // Act
            var result = await productRepository.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Electronics", result.First().Category.Name); 
        }
        [Fact]
        public async Task GetProducts_ReturnsEmpty_WhenNoProductsExist()
        {
            // Arrange
            var products = new List<Product>();

            var mockContext = new Mock<ApiDBContext>();
            mockContext
                .Setup(x => x.Products)
                .ReturnsDbSet(products);

            var productRepository = new ProductRepository(mockContext.Object);

            // Act
            var result = await productRepository.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


    }
}
