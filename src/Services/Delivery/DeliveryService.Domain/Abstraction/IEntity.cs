using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Abstraction
{
    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
    }
    public interface IEntity
    {
        string? CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string? LastModifiedOn { get; set; }
        DateTime? LastModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        string? DeletedBy { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
