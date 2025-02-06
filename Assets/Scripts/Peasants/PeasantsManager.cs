using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Peasants
{
    public class PeasantsManager : ISingleton<PeasantsManager>
    {
        public List<Peasant> _peasantsList;



        public Peasant GetClosestPeasantToPosition(Vector2 position)
        {
            _peasantsList.Sort((a, b) => Vector2.Distance(a.transform.position, position).CompareTo(Vector2.Distance(b.transform.position, position)));
            
            return _peasantsList[0];
        }
    }
}