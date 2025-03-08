using System;
using CDebugger;
using CustomObjectPool;
using Enemies.Factory;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public abstract class IEnemy : RecycableObject
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }


        internal override void Init()
        {
            CustomDebugger.Log(LogCategories.ObjectPool, $"Object Init {EnemyType.ToString()}");
        }

        internal override void Release()
        {
            CustomDebugger.Log(LogCategories.ObjectPool, $"{EnemyType} died, releasing memory spot");
        }
        
    }
}