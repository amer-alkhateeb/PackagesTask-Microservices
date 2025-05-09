namespace PackagesService.Application.Dtos
{
    public record CreatePackageRequest(string Sender, string Recipient, double Weight, string City, string Street, string Zip)
    {
    }
}
