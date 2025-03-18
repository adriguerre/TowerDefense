using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventBus
{
    public sealed class EventBus : ISingleton<EventBus>, DomainEventBus
    {
        readonly Dictionary<Type, List<Delegate>> subscribers = new ();
        readonly List<Delegate> subscribersToRemove = new ();
        
        public void Raise<T>(T ev) where T : DomainEvent
        {
            var childType = ev.GetType();
            if(!subscribers.ContainsKey(childType))
                return;
            
            RemoveDirty();
            foreach (var action in subscribers[childType].ToList())
                action.DynamicInvoke(ev);
            
            RemoveDirty();
        }

        public void Subscribe<T>(DomainEventHandler<T> handler) where T : DomainEvent
        {
            var action = new Action<T>(handler.Handle);
            if (IsSubscribed(action))
                throw new Exception("Cannot subscribe more than one domain event");
            
            Subscribe(action);
        }

        public void Unsubscribe<T>(DomainEventHandler<T> handler) where T : DomainEvent
        {
            RemoveDirty();
            var action = new Action<T>(handler.Handle);
            
            Unsubscribe(action);
        }

        public void Subscribe<T>(Action<T> action) where T : DomainEvent
        {
            RemoveDirty();
            if (IsSubscribed(action))
                throw new Exception("Cannot subscribe more than one domain event");

            if (!subscribers.ContainsKey(typeof(T)))
                subscribers.Add(typeof(T), new());
            subscribers[typeof(T)].Add(action);
        }
        
        public void Unsubscribe<T>(Action<T> action) where T : DomainEvent
        {
            if (!IsSubscribed(action) || !subscribers.Any())
                return;

            subscribersToRemove.Add(action);
        }

        void RemoveDirty()
        {
            foreach (var action in subscribersToRemove)
                foreach (var list in subscribers.Values.Where(list => list.Contains(action))) 
                    list.Remove(action);
            
            subscribersToRemove.Clear();
        }
        
        bool IsSubscribed<T>(Action<T> action) where T : DomainEvent
        {
            return subscribers.ContainsKey(typeof(T))
                   && subscribers[typeof(T)].Count(act => act == (Delegate)action)
                   > subscribersToRemove.Count(act => act == (Delegate)action);
        }
    }
}