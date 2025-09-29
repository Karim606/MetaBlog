using MetaBlog.Domain.Common.Events;
using MetaBlog.Domain.Common.Events.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Common
{
    public class Entity
    {
        public Guid Id { get; }

        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> domainEvents => _domainEvents.AsReadOnly();

        protected Entity() { }

        protected Entity(Guid id) {  Id = id==Guid.Empty?Guid.NewGuid():id; }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

    }
}
