using PackagesService.Application.CQRS;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Application.Packages.Commands
{
    public sealed record CreatePackageCommand(string Sender , string Recipient , double Weight , string City , string Street , string ZIP): ICommand<CreatePacakgeCommandResponse>;


    public sealed record CreatePacakgeCommandResponse (PackageId PackageId);
}
