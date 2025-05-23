using Moq;
using PackagesService.Application.Interfaces;
using PackagesService.Application.Packages.Commands;
using PackagesService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesService.Tests
{
    public class CreatePackageCommandHandlerTests
    {
        private readonly Mock<IPackageRepository> _mockRepository;
        private readonly CreatePackageCommandHandler _handler;

        public CreatePackageCommandHandlerTests()
        {
            _mockRepository = new Mock<IPackageRepository>();
            _handler = new CreatePackageCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesPackageAndReturnsResponse()
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                "Jane Smith",
                1.5,
                "New York",
                "123 Main St",
                "10001"
            );
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Package>(), cancellationToken))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesPackageWithCorrectProperties()
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                "Jane Smith",
                2.5,
                "Los Angeles",
                "456 Oak Ave",
                "90210"
            );
            var cancellationToken = CancellationToken.None;
            Package capturedPackage = null;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Package>(), cancellationToken))
                          .Callback<Package, CancellationToken>((pkg, ct) => capturedPackage = pkg)
                          .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(capturedPackage);
            Assert.Equal("John Doe", capturedPackage.Sender);
            Assert.Equal("Jane Smith", capturedPackage.Recipient);
            Assert.Equal(2.5, capturedPackage.Weight.Kilograms);
            Assert.Equal("456 Oak Ave", capturedPackage.Destination.Street);
            Assert.Equal("Los Angeles", capturedPackage.Destination.City);
            Assert.Equal("90210", capturedPackage.Destination.ZIP);
        }

        [Theory]
        [InlineData("", "Jane Smith", 1.5, "City", "Street", "12345")]
        [InlineData("   ", "Jane Smith", 1.5, "City", "Street", "12345")]
        [InlineData("John Doe", "", 1.5, "City", "Street", "12345")]
        [InlineData("John Doe", "   ", 1.5, "City", "Street", "12345")]
        public async Task Handle_InvalidSenderOrRecipient_ThrowsArgumentException(
            string sender, string recipient, double weight, string city, string street, string zip)
        {
            // Arrange
            var command = new CreatePackageCommand(sender, recipient, weight, city, street, zip);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(null, "Jane Smith", 1.5, "City", "Street", "12345")]
        [InlineData("John Doe", null, 1.5, "City", "Street", "12345")]
        public async Task Handle_InvalidSenderOrRecipient_ThrowsNullArgumentException(
    string sender, string recipient, double weight, string city, string street, string zip)
        {
            // Arrange
            var command = new CreatePackageCommand(sender, recipient, weight, city, street, zip);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command, cancellationToken));
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(0.05)]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        public async Task Handle_InvalidWeight_ThrowsArgumentException(double invalidWeight)
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                "Jane Smith",
                invalidWeight,
                "New York",
                "123 Main St",
                "10001"
            );
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData("John Doe", "Jane Smith", 1.5, "", "Street", "12345")]
        [InlineData("John Doe", "Jane Smith", 1.5, "   ", "Street", "12345")]
        [InlineData("John Doe", "Jane Smith", 1.5, "City", "", "12345")]
        [InlineData("John Doe", "Jane Smith", 1.5, "City", "   ", "12345")]
        [InlineData("John Doe", "Jane Smith", 1.5, "City", "Street", "")]
        [InlineData("John Doe", "Jane Smith", 1.5, "City", "Street", "   ")]
        public async Task Handle_InvalidAddressComponents_ThrowsArgumentException(
            string sender, string recipient, double weight, string city, string street, string zip)
        {
            // Arrange
            var command = new CreatePackageCommand(sender, recipient, weight, city, street, zip);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData("John Doe", "Jane Smith", 1.5, null, "Street", "12345")]
        [InlineData("John Doe", "Jane Smith", 1.5, "City", null, "12345")]
        [InlineData("John Doe", "Jane Smith", 1.5, "City", "Street", null)]

        public async Task Handle_InvalidAddressComponents_ThrowsNullArgumentException(
    string sender, string recipient, double weight, string city, string street, string zip)
        {
            // Arrange
            var command = new CreatePackageCommand(sender, recipient, weight, city, street, zip);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command, cancellationToken));
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                "Jane Smith",
                1.5,
                "New York",
                "123 Main St",
                "10001"
            );
            var cancellationToken = CancellationToken.None;
            var expectedException = new InvalidOperationException("Database error");

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Package>(), cancellationToken))
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, cancellationToken));
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public async Task Handle_CancellationRequested_PassesCancellationToken()
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                "Jane Smith",
                1.5,
                "New York",
                "123 Main St",
                "10001"
            );
            var cancellationToken = new CancellationToken(true);

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Package>(), cancellationToken))
                          .Returns(Task.CompletedTask);

            // Act & Assert
            await _handler.Handle(command, cancellationToken);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_MinimumValidWeight_CreatesPackageSuccessfully()
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                "Jane Smith",
                0.1, // Minimum valid weight
                "New York",
                "123 Main St",
                "10001"
            );
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Package>(), cancellationToken))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Package>(), cancellationToken), Times.Once);
        }
    }

}
