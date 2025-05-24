using DeliveryService.API.Contracts.DTOs;
using DeliveryService.API.Controllers;
using DeliveryService.Application.DeliveryRoutes.Commands;
using DeliveryService.Application.DeliveryRoutes.Queries;
using DeliveryService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Tests.UnitTests
{
    public class RoutesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly RoutesController _controller;

        public RoutesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new RoutesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new CreateRouteRequest(
                TruckId: Guid.NewGuid(),
                DriverId: Guid.NewGuid(),
                PackageIds: new List<string> { "PKG001", "PKG002" },
                ScheduledDate: DateTime.UtcNow.AddDays(1)
            );

            var expectedId = Guid.NewGuid();
            var commandResponse = new ScheduleDeliveryRouteCommandResponse(expectedId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ScheduleDeliveryRouteCommand>(), default))
                        .ReturnsAsync(commandResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(expectedId, createdAtActionResult.RouteValues["id"]);

            var responseValue = Assert.IsAssignableFrom<object>(createdAtActionResult.Value);
            var idProperty = responseValue.GetType().GetProperty("id");
            Assert.Equal(expectedId, idProperty.GetValue(responseValue));

            _mediatorMock.Verify(m => m.Send(It.Is<ScheduleDeliveryRouteCommand>(cmd =>
                cmd.TruckId == request.TruckId &&
                cmd.DriverId == request.DriverId &&
                cmd.PackageIds.SequenceEqual(request.PackageIds) &&
                cmd.ScheduledDate == request.ScheduledDate), default), Times.Once);
        }

        [Fact]
        public async Task Create_EmptyPackageIds_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new CreateRouteRequest(
                TruckId: Guid.NewGuid(),
                DriverId: Guid.NewGuid(),
                PackageIds: new List<string>(),
                ScheduledDate: DateTime.UtcNow.AddDays(1)
            );

            var expectedId = Guid.NewGuid();
            var commandResponse = new ScheduleDeliveryRouteCommandResponse(expectedId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ScheduleDeliveryRouteCommand>(), default))
                        .ReturnsAsync(commandResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(expectedId, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Create_EmptyGuids_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new CreateRouteRequest(
                TruckId: Guid.Empty,
                DriverId: Guid.Empty,
                PackageIds: new List<string> { "PKG001" },
                ScheduledDate: DateTime.UtcNow
            );

            var expectedId = Guid.NewGuid();
            var commandResponse = new ScheduleDeliveryRouteCommandResponse(expectedId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ScheduleDeliveryRouteCommand>(), default))
                        .ReturnsAsync(commandResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(expectedId.ToString(), createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Create_MediatorThrowsException_PropagatesException()
        {
            // Arrange
            var request = new CreateRouteRequest(
                TruckId: Guid.NewGuid(),
                DriverId: Guid.NewGuid(),
                PackageIds: new List<string> { "PKG001" },
                ScheduledDate: DateTime.UtcNow.AddDays(1)
            );

            _mediatorMock.Setup(m => m.Send(It.IsAny<ScheduleDeliveryRouteCommand>(), default))
                        .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Create(request));
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOkWithRouteResponse()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var deliveryRouteDto = new DeliveryRouteDto(
                Id: routeId,
                TruckId: Guid.NewGuid(),
                DriverId: Guid.NewGuid(),
                ScheduledDate: DateTime.UtcNow.AddDays(1),
                Deliveries: new List<DeliveryDto>
                {
                    new DeliveryDto(Guid.NewGuid(), "PKG001", "InTransit", "TRK001"),
                    new DeliveryDto(Guid.NewGuid(), "PKG002", "Delivered", "TRK002")
                }
            );

            var queryResponse = new GetRouteByIdQueryResponse(deliveryRouteDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetRouteByIdQuery>(q => q.Id == routeId), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetById(routeId);

            // Assert
            var okResult = Assert.IsType<ActionResult<RouteResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var routeResponse = Assert.IsType<RouteResponse>(okObjectResult.Value);

            Assert.Equal(deliveryRouteDto.Id, routeResponse.Id);
            Assert.Equal(deliveryRouteDto.TruckId, routeResponse.TruckId);
            Assert.Equal(deliveryRouteDto.DriverId, routeResponse.DriverId);
            Assert.Equal(deliveryRouteDto.ScheduledDate, routeResponse.ScheduledDate);
            Assert.Equal(deliveryRouteDto.Deliveries.Count, routeResponse.Deliveries.Count);

            _mediatorMock.Verify(m => m.Send(It.Is<GetRouteByIdQuery>(q => q.Id == routeId), default), Times.Once);
        }

        [Fact]
        public async Task GetById_EmptyGuid_ReturnsOkWithRouteResponse()
        {
            // Arrange
            var routeId = Guid.Empty;
            var deliveryRouteDto = new DeliveryRouteDto(
                Id: routeId,
                TruckId: Guid.NewGuid(),
                DriverId: Guid.NewGuid(),
                ScheduledDate: DateTime.UtcNow,
                Deliveries: new List<DeliveryDto>()
            );

            var queryResponse = new GetRouteByIdQueryResponse(deliveryRouteDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetRouteByIdQuery>(q => q.Id == routeId), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetById(routeId);

            // Assert
            var okResult = Assert.IsType<ActionResult<RouteResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var routeResponse = Assert.IsType<RouteResponse>(okObjectResult.Value);

            Assert.Equal(Guid.Empty, routeResponse.Id);
        }

        [Fact]
        public async Task GetById_RouteWithNoDeliveries_ReturnsOkWithEmptyDeliveries()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var deliveryRouteDto = new DeliveryRouteDto(
                Id: routeId,
                TruckId: Guid.NewGuid(),
                DriverId: Guid.NewGuid(),
                ScheduledDate: DateTime.UtcNow.AddDays(1),
                Deliveries: new List<DeliveryDto>()
            );

            var queryResponse = new GetRouteByIdQueryResponse(deliveryRouteDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetRouteByIdQuery>(q => q.Id == routeId), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetById(routeId);

            // Assert
            var okResult = Assert.IsType<ActionResult<RouteResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var routeResponse = Assert.IsType<RouteResponse>(okObjectResult.Value);

            Assert.Empty(routeResponse.Deliveries);
        }

        [Fact]
        public async Task GetById_MediatorThrowsException_PropagatesException()
        {
            // Arrange
            var routeId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.Is<GetRouteByIdQuery>(q => q.Id == routeId), default))
                        .ThrowsAsync(new KeyNotFoundException("Route not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.GetById(routeId));
        }

        [Fact]
        public async Task GetById_MediatorReturnsNull_HandlesGracefully()
        {
            // Arrange
            var routeId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.Is<GetRouteByIdQuery>(q => q.Id == routeId), default))
                        .ReturnsAsync((GetRouteByIdQueryResponse)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.GetById(routeId));
        }
    }
}
