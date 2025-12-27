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
    public class UserRepositoryIntegrationTests : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly ApiDBContext _dbContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _userRepository = new UserRepository(_dbContext);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        [Fact]
        public async Task Login_ReturnsNull_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var user = new User { FirstName = "RealUser", Email = "real@test.com", Password = "CorrectPassword" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login("real@test.com", "WrongPassword123");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUsers_ReturnsEmptyList_WhenNoUsersInDatabase()
        {
            // Act
            var result = await _userRepository.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsers_ReturnsUsersWithOrders_FromDatabase()
        {
            // Arrange
            var user = new User { FirstName = "Alice", Email = "alice@db.com", Password = "123" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var order = new Order { UserId = user.Id, OrderDate = DateOnly.FromDateTime(DateTime.Now), OrderSum = 99.9 };
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetUsers();

            // Assert
            Assert.NotEmpty(result);
            var fetchedUser = result.First(u => u.Email == "alice@db.com");
            Assert.Single(fetchedUser.Orders);
        }

        [Fact]
        public async Task Login_ReturnsCorrectUser_WhenCredentialsMatch()
        {
            // Arrange
            var user = new User { FirstName = "LoginTest", Email = "login@test.com", Password = "SecretPassword" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login("login@test.com", "SecretPassword");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("LoginTest", result.FirstName);
        }

        [Fact]
        public async Task UpdateUser_ActuallyPersistsChangesInDatabase()
        {
            // Arrange
            var user = new User { FirstName = "Before", Email = "before@test.com", Password = "123" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(user).State = EntityState.Detached;

            var userToUpdate = new User
            {
                Id = user.Id,
                FirstName = "After",
                Email = "after@test.com",
                Password = "123"
            };

            // Act
            await _userRepository.UpdateUser(user.Id, userToUpdate);

            // Assert
            _dbContext.ChangeTracker.Clear();
            var updatedUser = await _dbContext.Users.FindAsync(user.Id);

            Assert.NotNull(updatedUser);
            Assert.Equal("After", updatedUser.FirstName);
            Assert.Equal("after@test.com", updatedUser.Email);
        }

        [Fact]
        public async Task UserWithSameEmail_CorrectlyIdentifiesDuplicates()
        {
            // Arrange
            var existingUser = new User { FirstName = "Existing", Email = "taken@test.com", Password = "123" };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            // Act
            var isAvailableForNew = await _userRepository.UserWithSameEmail("taken@test.com", -1);
            var isAvailableForSelf = await _userRepository.UserWithSameEmail("taken@test.com", existingUser.Id);

            // Assert
            Assert.False(isAvailableForNew);
            Assert.True(isAvailableForSelf);
        }

        [Fact]
        public async Task AddUser_SavesUserCorrectly_AndGeneratesId()
        {
            // Arrange
            var newUser = new User
            {
                FirstName = "New",
                LastName = "User",
                Email = "new@test.com",
                Password = "Password123"
            };

            // Act
            var result = await _userRepository.AddUser(newUser);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal("New", result.FirstName);

            var userInDb = await _dbContext.Users.FindAsync(result.Id);
            Assert.NotNull(userInDb);
            Assert.Equal("new@test.com", userInDb.Email);
        }
    }
}