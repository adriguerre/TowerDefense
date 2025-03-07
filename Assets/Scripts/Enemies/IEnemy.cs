using System;
using Enemies.Factory;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public abstract class IEnemy : MonoBehaviour
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }

    }
}