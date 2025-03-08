using System.Collections.Generic;
using CDebugger;
using UnityEngine;

namespace CustomObjectPool
{
    public class ObjectPool
    {
        private readonly RecycableObject _prefab;
        private readonly HashSet<RecycableObject> _instantiatedObjects;
        private Queue<RecycableObject> _recycledObjects;

        public ObjectPool(RecycableObject prefab)
        {
            _prefab = prefab;
            _instantiatedObjects = new HashSet<RecycableObject>();
        }

        public void Init(int numberOfInitialObjects)
        {
            _recycledObjects = new Queue<RecycableObject>(numberOfInitialObjects);

            for (int i = 0; i < numberOfInitialObjects; i++)
            {
                var instance = InstantiateNewInstance();
                instance.gameObject.SetActive(false);
                _recycledObjects.Enqueue(instance);
            }
        }


        private RecycableObject InstantiateNewInstance()
        {
            var instance = Object.Instantiate(_prefab);
            instance.Configure(this);
            return instance;
        }

        public T Spawn<T>(Vector2 spawnPosition)
        {
            var recycableObject = GetInstance();
            _instantiatedObjects.Add(recycableObject); 
            recycableObject.gameObject.SetActive(true);
            recycableObject.Init();
            return recycableObject.GetComponent<T>();
        }

        private RecycableObject GetInstance()
        {
            if (_recycledObjects.Count > 0)
            {
                return _recycledObjects.Dequeue();
            }

            var instance = InstantiateNewInstance();
            return instance;
        }

        public void RecycleGameObject(RecycableObject gameObjectToRecycle)
        {
            var wasInstantiated = _instantiatedObjects.Remove(gameObjectToRecycle);

            if (wasInstantiated)
            {
                gameObjectToRecycle.gameObject.SetActive(false);
                gameObjectToRecycle.Release();
                _recycledObjects.Enqueue(gameObjectToRecycle);
            }
        }
    }
}