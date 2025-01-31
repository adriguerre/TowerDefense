using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuildingsManager : ISingleton<CivilianBuildingsManager>
    {
        //We will use this dicctionary to have control over all the buildings in the map, we will use this to destroy buildings when neccesary
        public Dictionary<int, GameObject> CivilianBuildingsDictionary {get; private set;}


        protected override void Awake()
        {
            base.Awake();
            
            CivilianBuildingsDictionary = new Dictionary<int, GameObject>();
        }


        public void AddCivilianBuilding(int buildingIDInLevel, GameObject civilianBuilding)
        {
                CivilianBuildingsDictionary[buildingIDInLevel] =(civilianBuilding);
        }
    }
}