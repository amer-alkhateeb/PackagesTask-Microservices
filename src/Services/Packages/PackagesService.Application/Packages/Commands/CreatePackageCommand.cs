using PackagesService.Application.CQRS;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Application.Packages.Commands
{
    public sealed record CreatePackageCommand(string Sender , string Recipient , double Weight , string City , string Street , string Zip): ICommand<CreatePackageCommandResponse>;


    public sealed record CreatePackageCommandResponse(PackageId PackageId);
}
