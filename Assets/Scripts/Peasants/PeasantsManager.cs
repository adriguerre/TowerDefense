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
            // Filtrar la lista para incluir solo los campesinos que no están ocupados
            var availablePeasants = _peasantsList.Where(p => !p.isBusy).ToList();
    
            // Verificar si hay campesinos disponibles
            if (availablePeasants.Count == 0)
            {
                return null; // Retornar null si no hay campesinos disponibles
            }
    
            // Ordenar la lista de campesinos disponibles según la distancia a la posición dada
            availablePeasants.Sort((a, b) => Vector2.Distance(a.transform.position, position)
                .CompareTo(Vector2.Distance(b.transform.position, position)));
    
            return availablePeasants[0];
        }

    }
}