using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuildingsManager : ISingleton<CivilianBuildingsManager>
    {
        //We will use this dicctionary to have control over all the buildings in the map, we will use this to destroy buildings when neccesary
        public Dictionary<int, GameObject> CurrentCivilianBuildingsDictionary {get; private set;}
        
        public Dictionary<int, Vector2> AllCivilianBuildingsPositions { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            
            CurrentCivilianBuildingsDictionary = new Dictionary<int, GameObject>();
            AllCivilianBuildingsPositions = new Dictionary<int, Vector2>();
        }


        public void AddCivilianBuilding(int buildingIDInLevel, GameObject civilianBuilding)
        {
            CurrentCivilianBuildingsDictionary[buildingIDInLevel] =(civilianBuilding);
        }

        public void FillCivilianBuildingsDictionary(LevelSO levelInfo)
        {
            foreach (var civilianBuilding in levelInfo.CivilianBuildingGridPosisitions)
            {
                Vector2 centerPosition =
                    LevelGrid.Instance.GetCenterPositionFromCivilianBuilding(civilianBuilding.buildingId,
                        civilianBuilding.size);
                AllCivilianBuildingsPositions.Add(civilianBuilding.buildingId,centerPosition);
            }
        }

        public List<BlockInfo> GetCivilianBuildingToBlock()
        {
            List<BlockInfo> blockerList = new List<BlockInfo>();
            foreach (var ownedBuilding in CurrentCivilianBuildingsDictionary)
            {
                int buildingSize = LevelGrid.Instance.GetSizeFromBuildingID(ownedBuilding.Key);
                BlockInfo blockInfo = new BlockInfo(ownedBuilding.Key, buildingSize,
                    AllCivilianBuildingsPositions[ownedBuilding.Key]);
                blockerList.Add(blockInfo);
            }
            
            return blockerList;
        }
        
    }
    

}