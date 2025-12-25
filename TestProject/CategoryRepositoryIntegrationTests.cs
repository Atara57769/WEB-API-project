using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class CategoryRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly ApiDBContext _dbContext;
        private readonly CategoryRepository _categoryRepository;
        public CategoryRepositoryIntegrationTests(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _categoryRepository = new CategoryRepository(_dbContext);
        }
        [Fact]
        public async Task GetCategories_ReturnsEmpty_WhenNoDataExists()
        {
            // Arrange
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetCategories();

            // Assert
            Assert.Empty(result);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetCategories_WhenDataExists_ReturnsAllCategories()
        {
            _dbContext.Categories.RemoveRange(_dbContext.Categories);

            var testCategories = new List<Category>
            {
             new Category { Name = "Electronics" },
             new Category { Name = "Home Decor" },
             new Category { Name = "Fashion" }
            };

            await _dbContext.Categories.AddRangeAsync(testCategories);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, c => c.Name == "Electronics");
            Assert.Contains(result, c => c.Name == "Home Decor");
            Assert.Contains(result, c => c.Name == "Fashion");
        }
    }
}
