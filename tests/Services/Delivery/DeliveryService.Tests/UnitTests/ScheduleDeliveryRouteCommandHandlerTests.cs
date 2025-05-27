using DeliveryService.Application.DeliveryRoutes.Commands;
using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Tests.UnitTests
{
    public class ScheduleDeliveryRouteCommandHandlerTests
    {
        private readonly Mock<IDeliveryRouteRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ScheduleDeliveryRouteCommandHandler _handler;

        public ScheduleDeliveryRouteCommandHandlerTests()
        {
            _mockRepository = new Mock<IDeliveryRouteRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new ScheduleDeliveryRouteCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesRouteAndReturnsResponse()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string> { "PKG001", "PKG002" },
                DateTime.UtcNow.AddDays(1)
            );
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken))
                          .ReturnsAsync((DeliveryRoute route, CancellationToken ct) => route);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken))
                          .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task Handle_ValidCommand_AddsCorrectNumberOfDeliveries()
        {
            // Arrange
            var packageIds = new List<string> { "PKG001", "PKG002", "PKG003" };
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                packageIds,
                DateTime.UtcNow.AddDays(1)
            );
            var cancellationToken = CancellationToken.None;
            DeliveryRoute capturedRoute = null;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken))
                          .Callback<DeliveryRoute, CancellationToken>((route, ct) => capturedRoute = route)
                          .ReturnsAsync((DeliveryRoute route, CancellationToken ct) => route);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken))
                          .ReturnsAsync(1);

            // Act
            await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(capturedRoute);
            Assert.Equal(3, capturedRoute.Deliveries.Count);
            Assert.All(capturedRoute.Deliveries, delivery =>
                Assert.Contains(delivery.PackageId, packageIds));
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryAndUnitOfWork()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string> { "PKG001" },
                DateTime.UtcNow.AddDays(1)
            );
            var cancellationToken = CancellationToken.None;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken))
                          .ReturnsAsync((DeliveryRoute route, CancellationToken ct) => route);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken))
                          .ReturnsAsync(1);

            // Act
            await _handler.Handle(command, cancellationToken);

            // Assert
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string> { "PKG001" },
                DateTime.UtcNow.AddDays(1)
            );
            var cancellationToken = CancellationToken.None;
            var expectedException = new InvalidOperationException("Database error");

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken))
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, cancellationToken));
            Assert.Equal("Database error", exception.Message);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UnitOfWorkThrowsException_PropagatesException()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string> { "PKG001" },
                DateTime.UtcNow.AddDays(1)
            );
            var cancellationToken = CancellationToken.None;
            var expectedException = new InvalidOperationException("Save failed");

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken))
                                        .ReturnsAsync((DeliveryRoute route, CancellationToken ct) => route);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken))
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, cancellationToken));
            Assert.Equal("Save failed", exception.Message);
        }

        [Fact]
        public async Task Handle_EmptyPackageList_CreatesRouteWithNoDeliveries()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string>(),
                DateTime.UtcNow.AddDays(1)
            );
            var cancellationToken = CancellationToken.None;
            DeliveryRoute capturedRoute = null;

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<DeliveryRoute>(), cancellationToken))
                           .Callback<DeliveryRoute, CancellationToken>((route, ct) => capturedRoute = route)
                           .ReturnsAsync((DeliveryRoute route, CancellationToken ct) => route);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken))
                          .ReturnsAsync(1);

            // Act
            await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(capturedRoute);
            Assert.Empty(capturedRoute.Deliveries);
        }
    }
}
