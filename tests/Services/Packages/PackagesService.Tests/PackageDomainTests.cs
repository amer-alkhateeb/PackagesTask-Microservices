using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesService.Tests
{
    public class PackageDomainTests
    {
        [Fact]
        public void CreatePackage_WithValidData_ShouldSucceed()
        {
            // Arrange
            var sender = "John Doe";
            var recipient = "Jane Smith";
            var weight = Weight.Of(2.5);
            var destination = Address.Of("Berlin", "Main Street", "10115");

            // Act
            var package = Package.Of(sender, recipient, weight, destination);

            // Assert
            Assert.NotNull(package);
            Assert.NotNull(package.Id);
            Assert.NotEqual(package.Id.Value , Guid.Empty);
            Assert.Equal(sender, package.Sender);
            Assert.Equal(recipient, package.Recipient);
            Assert.Equal(weight, package.Weight);
            Assert.Equal(destination, package.Destination);
        }

        [Fact]
        public void CreatePackage_WithInvalidWeight_ShouldThrowException()
        {
            // Arrange
            var sender = "John Doe";
            var recipient = "Jane Smith";
            var invalidWeight = -1.0;
            var destination = Address.Of("Berlin", "Main Street", "10115");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                Package.Of(sender, recipient, Weight.Of(invalidWeight), destination));
        }
    }
}
