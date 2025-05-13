namespace DeliveryService.Application.Interfaces
{
    public interface IRepository<T, TId>
    {
        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync(T item, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(TId id, T item, CancellationToken cancellationToken = default);
        Task<T?> DeleteAsync(TId id, CancellationToken cancellationToken = default); // Soft delete
    }
}
