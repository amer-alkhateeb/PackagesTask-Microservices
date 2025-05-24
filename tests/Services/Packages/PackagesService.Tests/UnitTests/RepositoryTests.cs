using Moq;
using PackagesService.Application.Interfaces;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesService.Tests.UnitTests
{
    public class RepositoryTests
    {
        private readonly Mock<IPackageRepository> _repositoryMock;

        public RepositoryTests()
        {
            _repositoryMock = new Mock<IPackageRepository>();
        }

        [Fact]
        public async Task GetPackageById_WithExistingId_ShouldReturnPackage()
        {
            // Arrange
            var packageId = PackageId.Create();
            var expectedPackage = Package.Of(
                "John Doe",
               "Jane Smith",
                Weight.Of(2.5),
                 Address.Of("Berlin", "Main Street", "10115")
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(packageId.Value,CancellationToken.None))
                .ReturnsAsync(expectedPackage);

            // Act
            var result = await _repositoryMock.Object.GetByIdAsync(packageId.Value, CancellationToken.None);

            // Assert
            Assert.Equal(expectedPackage, result);
        }

        [Fact]
        public async Task GetPackageById_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var packageId = PackageId.Create();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(packageId.Value, CancellationToken.None))
                .ReturnsAsync((Package)null);

            // Act
            var result = await _repositoryMock.Object.GetByIdAsync(packageId.Value, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
