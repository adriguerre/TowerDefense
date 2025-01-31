using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuildingsManager : ISingleton<CivilianBuildingsManager>
    {
        //We will use this dicctionary to have control over all the buildings in the map, we will use this to destroy buildings when neccesary
        
        //Aqui no puede ser int la key, ya que no podemos poner dos farm a la vez
        public Dictionary<int, List<GameObject>> CivilianBuildingsDictionary {get; private set;}


        protected override void Awake()
        {
            base.Awake();
            
            CivilianBuildingsDictionary = new Dictionary<int, List<GameObject>>();
        }


        public void AddCivilianBuilding(int buildingIDInLevel, GameObject civilianBuilding)
        {
            if (!CivilianBuildingsDictionary.ContainsKey(buildingIDInLevel))
            {
                List<GameObject> civilianBuildingList = new List<GameObject>();
                civilianBuildingList.Add(civilianBuilding);
                CivilianBuildingsDictionary.Add(buildingIDInLevel, civilianBuildingList);
            }
            else
            {
                CivilianBuildingsDictionary[buildingIDInLevel].Add(civilianBuilding);
            }
                
        }
    }
}