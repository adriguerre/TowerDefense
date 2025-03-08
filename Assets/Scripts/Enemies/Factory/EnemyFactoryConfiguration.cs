using System;
using System.Collections.Generic;
using CDebugger;
using UnityEngine;

namespace Enemies.Factory
{
    [CreateAssetMenu(menuName = "Enemies/FactoryConfigutation")]
    public class EnemyFactoryConfiguration : ScriptableObject
    {
        public List<IEnemy> Enemies => _enemies;
        
        [SerializeField] private List<IEnemy> _enemies;
        
        private Dictionary<EnemyType, IEnemy> _idToEnemy;

        

        private void Awake()
        {
            _idToEnemy = new Dictionary<EnemyType, IEnemy>();
            foreach (var enemy in _enemies)
            {
                _idToEnemy.Add(enemy.EnemyType, enemy);
            }
        }
        public IEnemy GetEnemyByType(EnemyType type)
        {
            if(!_idToEnemy.TryGetValue(type, out var enemy))
            {
                CustomDebugger.Log(LogCategories.CivilianBuildings, $"There is not enemy of this type {type}");
            }

            return enemy;
        }
    }
}