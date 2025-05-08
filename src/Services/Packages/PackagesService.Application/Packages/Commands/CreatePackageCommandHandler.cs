using PackagesService.Application.CQRS;
using PackagesService.Application.Interfaces;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;
using System.Net.Http.Headers;

namespace PackagesService.Application.Packages.Commands
{
    public sealed class CreatePackageCommandHandler : ICommandHandler<CreatePackageCommand, CreatePacakgeCommandResponse>
    {
        private readonly IPackageRepository _packageRepository;

        public CreatePackageCommandHandler(IPackageRepository packagerepository)
        {
            _packageRepository = packagerepository;
        }
        public async Task<CreatePacakgeCommandResponse> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            Package package = Package.Of(
                request.Sender,
                request.Recipient,
                Weight.Of(request.Weight),
                Address.Of(request.Street, request.City, request.ZIP)
            );

            await _packageRepository.AddAsync(package, cancellationToken);
            return new CreatePacakgeCommandResponse(package.Id);
        }
    }

}
