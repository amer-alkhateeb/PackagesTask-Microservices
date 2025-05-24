using DeliveryService.Application.DeliveryRoutes.Commands;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Tests.UnitTests
{
    public class ScheduleDeliveryRouteValidatorTests
    {
        private readonly ScheduleDeliveryRouteValidator _validator;

        public ScheduleDeliveryRouteValidatorTests()
        {
            _validator = new ScheduleDeliveryRouteValidator();
        }

        [Fact]
        public void Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string> { "PKG001" },
                DateTime.UtcNow.AddDays(1)
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_EmptyTruckId_HasValidationError()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.Empty,
                Guid.NewGuid(),
                new List<string> { "PKG001" },
                DateTime.UtcNow.AddDays(1)
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TruckId);
        }

        [Fact]
        public void Validate_PastScheduledDate_HasValidationError()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string> { "PKG001" },
                DateTime.UtcNow.AddDays(-1)
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ScheduledDate);
        }

        [Fact]
        public void Validate_EmptyPackageIds_HasValidationError()
        {
            // Arrange
            var command = new ScheduleDeliveryRouteCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new List<string>(),
                DateTime.UtcNow.AddDays(1)
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PackageIds);
        }
    }
}
