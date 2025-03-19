using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocator
{
    /// <summary>
    /// To Get data
    /// ServiceLocator.Instance.Get<Class>.data
    ///
    /// To register data
    /// ServiceLocator.Instance.Register(data);
    /// </summary>
    public class ServiceLocator : ISingleton<ServiceLocator>
    {
        private readonly List<(Type type, object service)> services = new();
        private readonly List<(Type type, object service)> lastUpdated = new();


        public void Register<T>(T service)
        {
            var existingIndex = services.FindIndex(s => s.type == typeof(T));
            if(existingIndex >= 0)
                services[existingIndex] = (typeof(T), service);
            else
                services.Add((typeof(T), service));
        }

        public void RegisterToList<T>(T service)
        {
            var existingIndex = services.FindIndex(s => s.type == typeof(List<T>));
            if (existingIndex >= 0)
            {
                var list = (List<T>)services[existingIndex].service;
                var itemIndex = list.FindIndex(s => s.Equals(service));
                if(itemIndex >= 0)
                    list[itemIndex] = service;
                else
                    list.Add(service);
                
                lastUpdated.RemoveAll(s => s.type == typeof(T));
                lastUpdated.Add((typeof(T), service));
            }
            else
            {
                var newList = new List<T>() {service};
                services.Add((typeof(List<T>), newList));
                
                lastUpdated.Add((typeof(T), service));
            }
        }



        public T Get<T>()
        {
            var entry = services.Find(s => s.type == typeof(List<T>));
            if (entry.service != null)
                return (T)entry.service;
            
            throw new InvalidOperationException($"No service of type {typeof(T).Name} has been registered");
        }

        public List<T> GetList<T>()
        {
            var entry = services.Find(s => s.type == typeof(List<T>));
            return entry.service != null ? (List<T>)entry.service : new List<T>();
        }

        public T GetLastUpdated<T>()
        {
            var entry = lastUpdated.Find(s => s.type == typeof(T));
            if (entry.service != null)
                return (T)entry.service;
            
            throw new InvalidOperationException($"No service of type {typeof(T).Name} has been registered or updated");

        }
    }
}