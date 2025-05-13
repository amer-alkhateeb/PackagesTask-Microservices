using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Abstraction
{
    public abstract class Entity<TId> : IEntity<TId>
    {
        public  TId Id { get; set; }
        public  string? CreatedBy { get; set; } = "System";
        public  DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public  string? LastModifiedOn { get; set; } = "System";
        public  DateTime? LastModifiedBy { get; set; } = DateTime.UtcNow;
        public  bool IsDeleted { get; set; } = false;
        public  string? DeletedBy { get; set; } = null;
        public  DateTime? DeletedOn { get; set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedOn = DateTime.UtcNow;
        }
    }
}
