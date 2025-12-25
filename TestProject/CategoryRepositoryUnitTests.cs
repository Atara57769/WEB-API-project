using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;

namespace TestProject
{
    public class CategoryRepositoryUnitTests
    {
        [Fact]
        public async Task GetCategories_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Books" }
        };

            var mockContext = new Mock<ApiDBContext>();
            mockContext
                .Setup(x => x.Categories)
                .ReturnsDbSet(categories);

            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCategories_WhenEmpty_ReturnsNoItems()
        {
            // Arrange
            var categories = new List<Category>(); // Empty list

            var mockContext = new Mock<ApiDBContext>();
            mockContext
                .Setup(x => x.Categories)
                .ReturnsDbSet(categories);

            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.Empty(result);
            Assert.NotNull(result);
        }
    }
}
