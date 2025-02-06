using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class InjectorManager : ISingleton<InjectorManager>
    {
        [Header("Builders")]
        public GameObject BuilderPrefab;
        public Vector2 BuildersOffsetToBuilding;
        public Vector2 BuildersOffsetToMoveAwayFromBuilding;
    }
}