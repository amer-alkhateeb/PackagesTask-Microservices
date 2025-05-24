using DeliveryService.API.Contracts.DTOs;
using DeliveryService.API.Controllers;
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
    public class DeliveriesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeliveriesController _controller;

        public DeliveriesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new DeliveriesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetByTrackingCode_ValidTrackingCode_ReturnsOkWithDeliveryResponse()
        {
            // Arrange
            var trackingCode = "TRK12345";
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "InTransit",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow.AddHours(2),
                ActualTime: null
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var deliveryResponse = Assert.IsType<DeliveryResponse>(okObjectResult.Value);

            Assert.Equal(deliveryDetailsDto.Id, deliveryResponse.Id);
            Assert.Equal(deliveryDetailsDto.PackageId, deliveryResponse.PackageId);
            Assert.Equal(deliveryDetailsDto.Status, deliveryResponse.Status);
            Assert.Equal(deliveryDetailsDto.TrackingCode, deliveryResponse.TrackingCode);
            Assert.Equal(deliveryDetailsDto.EstimatedTime, deliveryResponse.EstimatedTime);
            Assert.Equal(deliveryDetailsDto.ActualTime, deliveryResponse.ActualTime);

            _mediatorMock.Verify(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default), Times.Once);
        }

        [Fact]
        public async Task GetByTrackingCode_DeliveredPackage_ReturnsOkWithActualTime()
        {
            // Arrange
            var trackingCode = "TRK12345";
            var actualDeliveryTime = DateTime.UtcNow.AddHours(-1);
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "Delivered",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow.AddHours(2),
                ActualTime: actualDeliveryTime
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var deliveryResponse = Assert.IsType<DeliveryResponse>(okObjectResult.Value);

            Assert.Equal("Delivered", deliveryResponse.Status);
            Assert.Equal(actualDeliveryTime, deliveryResponse.ActualTime);
        }

        [Fact]
        public async Task GetByTrackingCode_EmptyTrackingCode_ReturnsOkWithResponse()
        {
            // Arrange
            var trackingCode = "";
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "Unknown",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow,
                ActualTime: null
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var deliveryResponse = Assert.IsType<DeliveryResponse>(okObjectResult.Value);

            Assert.Equal("", deliveryResponse.TrackingCode);
        }

        [Fact]
        public async Task GetByTrackingCode_WhitespaceTrackingCode_ReturnsOkWithResponse()
        {
            // Arrange
            var trackingCode = "   ";
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "Unknown",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow,
                ActualTime: null
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.IsType<DeliveryResponse>(okObjectResult.Value);
        }

        [Fact]
        public async Task GetByTrackingCode_NonExistentTrackingCode_ReturnsOkWithNullResponse()
        {
            // Arrange
            var trackingCode = "NONEXISTENT123";

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync((GetDeliveryByTrackingCodeQueryResponse)null);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);

            // Since the controller calls .Adapt<DeliveryResponse>() on null, this will throw
            // This test verifies the current behavior - you might want to add null checking in the controller
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _controller.GetByTrackingCode(trackingCode);
            });
        }

        [Fact]
        public async Task GetByTrackingCode_SpecialCharactersInTrackingCode_ReturnsOkWithResponse()
        {
            // Arrange
            var trackingCode = "TRK-123@#$%";
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "InTransit",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow.AddHours(2),
                ActualTime: null
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var deliveryResponse = Assert.IsType<DeliveryResponse>(okObjectResult.Value);

            Assert.Equal(trackingCode, deliveryResponse.TrackingCode);
        }

        [Fact]
        public async Task GetByTrackingCode_VeryLongTrackingCode_ReturnsOkWithResponse()
        {
            // Arrange
            var trackingCode = new string('A', 1000); // Very long tracking code
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "InTransit",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow.AddHours(2),
                ActualTime: null
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var deliveryResponse = Assert.IsType<DeliveryResponse>(okObjectResult.Value);

            Assert.Equal(trackingCode, deliveryResponse.TrackingCode);
        }

        [Fact]
        public async Task GetByTrackingCode_MediatorThrowsException_PropagatesException()
        {
            // Arrange
            var trackingCode = "TRK12345";

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ThrowsAsync(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetByTrackingCode(trackingCode));
        }

        [Fact]
        public async Task GetByTrackingCode_MediatorThrowsArgumentException_PropagatesException()
        {
            // Arrange
            var trackingCode = "INVALID";

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ThrowsAsync(new ArgumentException("Invalid tracking code format"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _controller.GetByTrackingCode(trackingCode));
        }

        [Theory]
        [InlineData("TRK123")]
        [InlineData("DELIVERY456")]
        [InlineData("PKG-789")]
        [InlineData("1234567890")]
        public async Task GetByTrackingCode_VariousValidTrackingCodes_ReturnsOkWithResponse(string trackingCode)
        {
            // Arrange
            var deliveryDetailsDto = new DeliveryDetailsDto(
                Id: Guid.NewGuid(),
                PackageId: "PKG001",
                Status: "InTransit",
                TrackingCode: trackingCode,
                EstimatedTime: DateTime.UtcNow.AddHours(2),
                ActualTime: null
            );

            var queryResponse = new GetDeliveryByTrackingCodeQueryResponse(deliveryDetailsDto);

            _mediatorMock.Setup(m => m.Send(It.Is<GetDeliveryByTrackingCodeQuery>(q => q.TrackingCode == trackingCode), default))
                        .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetByTrackingCode(trackingCode);

            // Assert
            var okResult = Assert.IsType<ActionResult<DeliveryResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var deliveryResponse = Assert.IsType<DeliveryResponse>(okObjectResult.Value);

            Assert.Equal(trackingCode, deliveryResponse.TrackingCode);
        }
    }

}
