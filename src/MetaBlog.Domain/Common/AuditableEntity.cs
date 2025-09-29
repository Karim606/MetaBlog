using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Common
{
    public abstract class AuditableEntity:Entity
    {
        protected AuditableEntity() : base() { }
        protected AuditableEntity(Guid id) : base(id) { }

        public DateTimeOffset createdAt { get; set; }
        public string createdBy { get; set; } = string.Empty;
        public DateTimeOffset? lastModifiedAt { get; set; }
        public string? lastModifiedBy { get; set; }
    }
}
