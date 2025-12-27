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
    public class CategoryRepositoryIntegrationTests : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly ApiDBContext _dbContext;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _categoryRepository = new CategoryRepository(_dbContext);
        }
        public void Dispose()
        {
            _fixture.Dispose();
        }

        [Fact]
        public async Task GetCategories_ReturnsEmpty_WhenNoDataExists()
        {
            var result = await _categoryRepository.GetCategories();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCategories_WhenDataExists_ReturnsAllCategories()
        {
            // Arrange
            var testCategories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Fashion" }
            };
            await _dbContext.Categories.AddRangeAsync(testCategories);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetCategories();

            // Assert
            Assert.Equal(2, result.Count());
        }

        
    }
}