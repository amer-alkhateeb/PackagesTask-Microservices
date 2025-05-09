using PackagesService.Application.CQRS;
using PackagesService.Application.Interfaces;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Application.Packages.Commands
{
    public sealed class CreatePackageCommandHandler : ICommandHandler<CreatePackageCommand, CreatePackageCommandResponse>
    {
        private readonly IPackageRepository _packageRepository;

        public CreatePackageCommandHandler(IPackageRepository packagerepository)
        {
            _packageRepository = packagerepository;
        }
        public async Task<CreatePackageCommandResponse> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            Package package = Package.Of(
                request.Sender,
                request.Recipient,
                Weight.Of(request.Weight),
                Address.Of(request.Street, request.City, request.Zip)
            );

            await _packageRepository.AddAsync(package, cancellationToken);
            return new CreatePackageCommandResponse(package.Id);
        }
    }

}
