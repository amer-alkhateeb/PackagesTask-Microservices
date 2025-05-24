using FluentValidation.TestHelper;
using PackagesService.Application.Packages.Commands;

namespace PackagesService.Tests.UnitTests
{
    public class CreatePackageCommandValidatorTests
    {
        private readonly CreatePackageCommandValidator _validator;

        public CreatePackageCommandValidatorTests()
        {
            _validator = new CreatePackageCommandValidator();
        }

        [Fact]
        public void Validate_ValidCommand_PassesValidation()
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

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_InvalidSender_HasValidationError(string invalidSender)
        {
            // Arrange
            var command = new CreatePackageCommand(
                invalidSender,
                "Jane Smith",
                1.5,
                "New York",
                "123 Main St",
                "10001"
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Sender)
                  .WithErrorMessage("Sender is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_InvalidRecipient_HasValidationError(string invalidRecipient)
        {
            // Arrange
            var command = new CreatePackageCommand(
                "John Doe",
                invalidRecipient,
                1.5,
                "New York",
                "123 Main St",
                "10001"
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Recipient)
                  .WithErrorMessage("Sender is recipient.");
        }

        [Fact]
        public void Validate_BothSenderAndRecipientInvalid_HasBothValidationErrors()
        {
            // Arrange
            var command = new CreatePackageCommand(
                "",
                "",
                1.5,
                "New York",
                "123 Main St",
                "10001"
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Sender);
            result.ShouldHaveValidationErrorFor(x => x.Recipient);
        }
    }
}
