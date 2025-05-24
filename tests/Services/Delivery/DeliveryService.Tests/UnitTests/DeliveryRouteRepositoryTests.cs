using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Models;
using Moq;

namespace DeliveryService.Tests.UnitTests
{
    public class DeliveryRouteRepositoryTests
    {
        private readonly Mock<IDeliveryRouteRepository> _repositoryMock;

        public DeliveryRouteRepositoryTests()
        {
            _repositoryMock = new Mock<IDeliveryRouteRepository>();
        }


        [Fact]
        public async Task GetByTrackingCodeAsync_WithNonExistingTrackingCode_ShouldReturnNull()
        {
            // Arrange
            var trackingCode = "NONEXISTENT";

            _repositoryMock.Setup(repo => repo.GetByTrackingCodeAsync(trackingCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Delivery)null);

            // Act
            var result = await _repositoryMock.Object.GetByTrackingCodeAsync(trackingCode, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
