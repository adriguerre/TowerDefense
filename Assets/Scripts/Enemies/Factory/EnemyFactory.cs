using CDebugger;
using UnityEngine;

namespace Enemies.Factory
{
    public class EnemyFactory
    {
        private readonly EnemyFactoryConfiguration _factoryConfiguration;
        
        public EnemyFactory(EnemyFactoryConfiguration factoryConfiguration)
        {
            this._factoryConfiguration = factoryConfiguration;
        }

        public IEnemy Create(EnemyType enemyType)
        {
            var enemy = _factoryConfiguration.GetEnemyByType(enemyType);
            CustomDebugger.Log(LogCategories.Enemies, $"Spawn enemy of type {enemyType}");
            return Object.Instantiate(enemy);
        }
        
    }
}