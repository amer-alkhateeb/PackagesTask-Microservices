using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PackagesService.API.Controllers;
using PackagesService.Application.Dtos;
using PackagesService.Application.Interfaces;
using PackagesService.Application.Packages.Commands;
using PackagesService.Application.Packages.Queries;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesService.Tests.UnitTests
{
    public class PackageControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PackagesController _controller;

        public PackageControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PackagesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsPackage()
        {
            // Arrange
            var id = PackageId.Create();

            var queryResult = new GetPackageByIdQueryResult(
                id.Value,
                "John",
                "Jane",
                2.5,
                "Amman",
                "Rainbow St",
                "11181"
            );

            _mediatorMock.Setup(m => m.Send(
                It.Is<GetPackageByIdQuery>(q => q.PackageId == id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _controller.GetById(id.Value);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<GetPackageByIdQueryResult>(okResult.Value);
            Assert.Equal(queryResult.Id, returned.Id);
        }

        [Fact]
        public async Task GetById_NonexistentId_ReturnsNotFound()
        {
            var id = PackageId.Create();

            _mediatorMock.Setup(m => m.Send(
                It.Is<GetPackageByIdQuery>(q => q.PackageId == id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetPackageByIdQueryResult)null);

            var result = await _controller.GetById(id.Value);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedResult()
        {
            var request = new CreatePackageRequest
            (
                 "Ali",
               "Sara",
              3,
               "Irbid",
                "Main St",
                 "22110"
            );

            var createdId = PackageId.Create();
            var response = new CreatePackageCommandResponse(createdId.Value);
            _mediatorMock.Setup(m => m.Send(
     It.Is<CreatePackageCommand>(cmd =>
         cmd.Sender == request.Sender &&

         cmd.Recipient == request.Recipient &&

         cmd.Weight == request.Weight &&
         cmd.City == request.City &&
         cmd.Street == request.Street &&
         cmd.Zip == request.Zip),
     It.IsAny<CancellationToken>()))
     .ReturnsAsync(response);

            var result = await _controller.Create(request);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Create", createdAt.ActionName);
            Assert.Equal(createdId.Value, createdAt.RouteValues["id"]);
        }

    }
}
