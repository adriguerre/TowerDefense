using UnityEngine;

namespace CustomObjectPool
{
    public abstract class RecycableObject : MonoBehaviour
    {
        private ObjectPool _objectPool;

        internal void Configure(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public void Recycle()
        {
            _objectPool.RecycleGameObject(this);
        }

        internal abstract void Init();

        internal abstract void Release();
    }
}