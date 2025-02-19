using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings.MilitaryBuildings
{
    public class MilitaryBuildingsManager : ISingleton<MilitaryBuildingsManager>
    {
        /// <summary>
        /// Key - Position
        /// Value - Building Gameobject
        /// </summary>
        public Dictionary<GridPosition, GameObject> CurrentBuildingsDictionary {get; private set;}


        private void Awake()
        {
            CurrentBuildingsDictionary = new Dictionary<GridPosition, GameObject>();
        }

        public void AddMilitaryBuildingsPosition(GridPosition gridPosition, GameObject building)
        {
            if (!CurrentBuildingsDictionary.ContainsKey(gridPosition))
            {
                CurrentBuildingsDictionary.Add(gridPosition, building);
            }
        }
    }
}