using System.Collections.Generic;
using CDebugger;
using CustomObjectPool;
using UnityEngine;

namespace Enemies.Factory
{
    public class EnemyFactory
    {
        private readonly EnemyFactoryConfiguration _factoryConfiguration;

        private Dictionary<EnemyType, ObjectPool> _pools;
        
        public EnemyFactory(EnemyFactoryConfiguration factoryConfiguration)
        {
            _pools = new Dictionary<EnemyType, ObjectPool>();
            
            this._factoryConfiguration = factoryConfiguration;
            var enemies = this._factoryConfiguration.Enemies;
            
            foreach (var enemy in enemies)
            {
                var objectPool = new ObjectPool(enemy);
                objectPool.Init(0);
                _pools.Add(enemy.EnemyType, objectPool);
            }
        }

        public IEnemy Create(EnemyType enemyType, Vector2 position)
        {
            var objectPool = _pools[enemyType];
            CustomDebugger.Log(LogCategories.ObjectPoolFactory, $"Spawn enemy of type {enemyType} with object pool");
            return objectPool.Spawn<IEnemy>(position);
        }
        
    }
}