using PackagesService.Application.CQRS;
using PackagesService.Application.Interfaces;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Application.Packages.Queries
{
    public sealed class GetPackageByIdQueryHandler : IQueryHandler<GetPackageByIdQuery, GetPackageByIdQueryResult>
    {

        private readonly IPackageRepository _packageRepository;
        public GetPackageByIdQueryHandler(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }
        public async Task<GetPackageByIdQueryResult> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _packageRepository.GetByIdAsync (request.PackageId.Value, cancellationToken);

            return new GetPackageByIdQueryResult (result.Id.Value, result.Sender, result.Recipient, result.Weight.Kilograms, result.Destination.Street, result.Destination.City, result.Destination.ZIP);
        }
    }
}
