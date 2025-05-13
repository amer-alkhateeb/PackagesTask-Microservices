using MediatR;

namespace DeliveryService.Application.CQRS
{
    public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse
        : notnull
    {
    }

    public interface ICommand : IRequest<Unit>
    {
    }
}
