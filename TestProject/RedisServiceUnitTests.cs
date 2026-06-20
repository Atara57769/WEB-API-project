using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class RedisServiceUnitTests
    {
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly Mock<ILogger<RedisService>> _mockLogger;
        private readonly RedisService _redisService;

        public RedisServiceUnitTests()
        {
            _mockCache = new Mock<IDistributedCache>();
            _mockLogger = new Mock<ILogger<RedisService>>();
            _redisService = new RedisService(_mockCache.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsDeserializedObject_WhenKeyExists()
        {
            // Arrange
            var key = "test-key";
            var expectedValue = new TestObject { Name = "Test" };
            var json = JsonSerializer.Serialize(expectedValue);
            
            _mockCache.Setup(c => c.GetAsync(key, default))
                .ReturnsAsync(System.Text.Encoding.UTF8.GetBytes(json));

            // Act
            var result = await _redisService.GetAsync<TestObject>(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedValue.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = "non-existent-key";
            _mockCache.Setup(c => c.GetAsync(key, default))
                .ReturnsAsync((byte[]?)null);

            // Act
            var result = await _redisService.GetAsync<TestObject>(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SetAsync_SerializesAndSetsValue()
        {
            // Arrange
            var key = "test-key";
            var value = new TestObject { Name = "Test" };

            // Act
            await _redisService.SetAsync(key, value);

            // Assert
            _mockCache.Verify(c => c.SetAsync(
                key,
                It.Is<byte[]>(b => System.Text.Encoding.UTF8.GetString(b).Contains("Test")),
                It.IsAny<DistributedCacheEntryOptions>(),
                default), Times.Once);
        }

        [Fact]
        public async Task GetStringAsync_ReturnsString_WhenKeyExists()
        {
            // Arrange
            var key = "test-string-key";
            var expectedValue = "hello";
            _mockCache.Setup(c => c.GetAsync(key, default))
                .ReturnsAsync(System.Text.Encoding.UTF8.GetBytes(expectedValue));

            // Act
            var result = await _redisService.GetStringAsync(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public async Task SetStringAsync_SetsRawString()
        {
            // Arrange
            var key = "test-string-key";
            var value = "hello";

            // Act
            await _redisService.SetStringAsync(key, value);

            // Assert
            _mockCache.Verify(c => c.SetAsync(
                key,
                It.Is<byte[]>(b => System.Text.Encoding.UTF8.GetString(b) == value),
                It.IsAny<DistributedCacheEntryOptions>(),
                default), Times.Once);
        }

        private class TestObject
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}
