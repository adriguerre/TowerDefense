using UnityEngine;

namespace DependencyInjection
{
    /// <summary>
    /// Responsible for initialisation and dependency resolution for the entire scene
    /// </summary>
    public class ResolveScene : MonoBehaviour
    {
        void Awake()
        {
            var dependencyResolver = new DependencyResolver();
            dependencyResolver.ResolveScene();
        }
    }
}

