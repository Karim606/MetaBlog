using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Common.Events.Interface;
namespace MetaBlog.Application.Common
{
    public class MediatREventDomainNotification<TDomainEvent>:INotification where TDomainEvent: IDomainEvent
    {
        public TDomainEvent DomainEvent { get; }
        public MediatREventDomainNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));
        }
    }
}
