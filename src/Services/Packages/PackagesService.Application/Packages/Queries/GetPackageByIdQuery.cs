using PackagesService.Application.CQRS;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Application.Packages.Queries
{
    public sealed record GetPackageByIdQuery(PackageId PackageId) : IQuery<GetPackageByIdQueryResult>
    {

    }

    public sealed record GetPackageByIdQueryResult(Guid Id,string Sender, string Recipient, double Weight, string City, string Street, string ZIP);

}
