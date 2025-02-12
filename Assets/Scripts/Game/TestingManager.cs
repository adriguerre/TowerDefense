using UnityEngine;

namespace Game
{
    public class TestingManager : ISingleton<TestingManager>
    {
        [field: SerializeField] public bool ResourcesNotNeeded {get; private set;}
    }
}