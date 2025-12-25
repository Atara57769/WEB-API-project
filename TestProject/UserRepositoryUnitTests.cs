using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UserRepositoryUnitTests
    {
        [Fact]
        public async Task GetUsers_ReturnsAllUsersWithOrders()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Alice",
                    Email = "alice@test.com",
                    Orders = new List<Order> { new Order { Id = 101, OrderSum = 50.0 } }
                },
                new User
                {
                    Id = 2,
                    FirstName = "Bob",
                    Email = "bob@test.com",
                    Orders = new List<Order>()
                }
            };

            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var repository = new UserRepository(mockContext.Object);

            // Act
            var result = await repository.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var alice = result.FirstOrDefault(u => u.FirstName == "Alice");
            Assert.NotNull(alice);
            Assert.Single(alice.Orders);

            var bob = result.FirstOrDefault(u => u.FirstName == "Bob");
            Assert.Empty(bob.Orders);
        }
        [Fact]
        public async Task GetUsers_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            var repository = new UserRepository(mockContext.Object);

            // Act
            var result = await repository.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task Login_ReturnsUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var users = new List<User>
            {
                new User { Id = 1, Email = email, Password = password, FirstName = "John" },
                new User { Id = 2, Email = "other@test.com", Password = "wrong" }
            };

            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var repository = new UserRepository(mockContext.Object);

            // Act
            var result = await repository.Login(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task Login_ReturnsNull_WhenCredentialsAreWrong()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "test@test.com", Password = "correct" }
            };

            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var repository = new UserRepository(mockContext.Object);

            // Act
            var result = await repository.Login("test@test.com", "wrong_password");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddUser_CallsAddAndSave_ReturnsNewUser()
        {
            // Arrange
            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            var repository = new UserRepository(mockContext.Object);
            var newUser = new User { Email = "new@user.com", Password = "789", FirstName = "Jane" };

            // Act
            var result = await repository.AddUser(newUser);

            // Assert
            Assert.NotNull(result);
            mockContext.Verify(x => x.Users.AddAsync(It.IsAny<User>(), default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UserWithSameEmail_ReturnsTrue_WhenEmailIsUnique()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "existing@test.com" }
            };

            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var repository = new UserRepository(mockContext.Object);

            // Act
            var isUnique = await repository.UserWithSameEmail("unique@test.com", -1);

            // Assert
            Assert.True(isUnique);
        }

        [Fact]
        public async Task UserWithSameEmail_ReturnsFalse_WhenEmailAlreadyExistsForAnotherUser()
        {
            // Arrange
            var email = "duplicate@test.com";
            var users = new List<User>
            {
                new User { Id = 1, Email = email },
                new User { Id = 2, Email = "other@test.com" }
            };

            var mockContext = new Mock<ApiDBContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var repository = new UserRepository(mockContext.Object);

            // Act
            var isUnique = await repository.UserWithSameEmail(email, 2);

            // Assert
            Assert.False(isUnique);
        }
        [Fact]
        public async Task UpdateUser_ShouldUpdateEmailAndFirstName()
        {
            // Arrange
            var mockContext = new Mock<ApiDBContext>();

            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            var repository = new UserRepository(mockContext.Object);

            var updatedUser = new User
            {
                Id = 1,
                FirstName = "NewName",
                Email = "new@test.com",
                Password = "123"
            };

            // Act
            await repository.UpdateUser(1, updatedUser);

            // Assert
            mockContext.Verify(x => x.Users.Update(It.Is<User>(u =>
                u.FirstName == "NewName" &&
                u.Email == "new@test.com")), Times.Once);

            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}
