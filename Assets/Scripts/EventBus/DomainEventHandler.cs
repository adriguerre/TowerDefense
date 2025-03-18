using UnityEngine;

namespace EventBus
{
    public interface DomainEventHandler<in T>
        where T : DomainEvent
    {
        void Handle(T ev);
    }
}