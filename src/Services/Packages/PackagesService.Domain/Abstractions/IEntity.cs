namespace PackagesService.Domain.Abstractions
{
    public interface IEntity<TId> : IEntity
    {
        public TId Id { get; set; }
    }
    public interface IEntity
    {
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
