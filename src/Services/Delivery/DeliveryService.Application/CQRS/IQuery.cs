using MediatR;

namespace DeliveryService.Application.CQRS
{
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
    {
    }
}
