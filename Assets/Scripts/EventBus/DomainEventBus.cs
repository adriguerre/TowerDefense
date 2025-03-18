using System;

namespace EventBus
{
    public interface DomainEventBus
    {
        void Raise<T>(T ev) where T : DomainEvent;
        
        void Subscribe<T>(DomainEventHandler<T> handler) where T : DomainEvent;
        void Unsubscribe<T>(DomainEventHandler<T> handler) where T : DomainEvent;
        void Subscribe<T>(Action<T> action) where T : DomainEvent;
        void Unsubscribe<T>(Action<T> action) where T : DomainEvent;
    }
}