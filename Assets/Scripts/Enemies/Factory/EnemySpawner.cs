using System;
using UnityEngine;

namespace Enemies.Factory
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyFactoryConfiguration _enemyFactoryConfiguration;

        private EnemyFactory _enemyFactory;

        private void Awake()
        {
            _enemyFactory = new EnemyFactory(Instantiate(_enemyFactoryConfiguration));
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _enemyFactory.Create(EnemyType.Infantry, Vector2.zero);
            }else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _enemyFactory.Create(EnemyType.Archer, Vector2.zero);
            }
        }
        
        #endif
    }
}