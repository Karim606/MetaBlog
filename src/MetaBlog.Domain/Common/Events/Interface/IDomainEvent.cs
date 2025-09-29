using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Common.Events.Interface
{
    public class IDomainEvent
    {
        public Guid Id { get; init; }
        public DateTime OccurredOn { get; init; }
        public string EventType => GetType().Name;
        
    }
}
