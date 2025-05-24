using Moq;
using PackagesService.Application.Interfaces;
using PackagesService.Application.Packages.Queries;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Tests.UnitTests
{
    public class GetPackageByIdQueryHandlerTests
    {
        private readonly Mock<IPackageRepository> _mockRepository;
        private readonly GetPackageByIdQueryHandler _handler;

        public GetPackageByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IPackageRepository>();
            _handler = new GetPackageByIdQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_PackageExists_ReturnsPackageData()
        {
            // Arrange
            var packageId = PackageId.Create();
            var package = Package.Of(
                "John Doe",
                "Jane Smith",
                Weight.Of(2.5),
                Address.Of("123 Main St", "New York", "10001")
            );
            var query = new GetPackageByIdQuery(packageId);
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.GetByIdAsync(packageId.Value, cancellationToken))
                          .ReturnsAsync(package);

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(package.Id.Value, result.Id);
            Assert.Equal("John Doe", result.Sender);
            Assert.Equal("Jane Smith", result.Recipient);
            Assert.Equal(2.5, result.Weight);
            Assert.Equal("123 Main St", result.Street);
            Assert.Equal("New York", result.City);
            Assert.Equal("10001", result.ZIP);
        }

        [Fact]
        public async Task Handle_PackageNotFound_ThrowsNullReferenceException()
        {
            // Arrange
            var packageId = PackageId.Create();
            var query = new GetPackageByIdQuery(packageId);
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.GetByIdAsync(packageId.Value, cancellationToken))
                          .ReturnsAsync((Package?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(query, cancellationToken));
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var packageId = PackageId.Create();
            var query = new GetPackageByIdQuery(packageId);
            var cancellationToken = CancellationToken.None;
            var expectedException = new InvalidOperationException("Database error");

            _mockRepository.Setup(x => x.GetByIdAsync(packageId.Value, cancellationToken))
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(query, cancellationToken));
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidQuery_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var packageId = PackageId.Create();
            var package = Package.Of(
                "Sender",
                "Recipient",
                Weight.Of(1.0),
                Address.Of("Street", "City", "ZIP")
            );
            var query = new GetPackageByIdQuery(packageId);
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.GetByIdAsync(packageId.Value, cancellationToken))
                          .ReturnsAsync(package);

            // Act
            await _handler.Handle(query, cancellationToken);

            // Assert
            _mockRepository.Verify(x => x.GetByIdAsync(packageId.Value, cancellationToken), Times.Once);
        }
    }
}
