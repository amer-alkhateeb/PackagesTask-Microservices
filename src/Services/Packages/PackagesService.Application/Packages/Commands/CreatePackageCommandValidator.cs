using FluentValidation;

namespace PackagesService.Application.Packages.Commands
{
    public sealed class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
    
    
    {

        public CreatePackageCommandValidator() { 
        
            RuleFor(x=>x.Sender).NotEmpty().WithMessage("Sender is required.");
            RuleFor(x=>x.Recipient).NotEmpty().WithMessage("Sender is recipient.");
        }
    }
}
