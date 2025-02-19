using System.Collections.Generic;
using Buildings.CivilianBuildings;
using UnityEngine;

namespace BuildingsTest
{
    public class IBuildingManager : MonoBehaviour
    {
        /// <summary>
        /// Key - Building ID
        /// Value - Its Gameobject
        /// </summary>
        public Dictionary<int, GameObject> CurrentBuildingsDictionary {get; private set;}
        /// <summary>
        /// Key - Building id from all buildings in level
        /// Value - Position
        /// </summary>
        public Dictionary<int, Vector2> AllBuildingsPositions { get; private set; }
        
        protected virtual void Awake()
        {
            CurrentBuildingsDictionary = new Dictionary<int, GameObject>();
            AllBuildingsPositions = new Dictionary<int, Vector2>();
        }
        public void AddCivilianBuilding(int buildingIDInLevel, GameObject civilianBuilding)
        {
            CurrentBuildingsDictionary[buildingIDInLevel] =(civilianBuilding);
        }
        
        public virtual void FillCivilianBuildingsDictionary(LevelSO levelInfo)
        {
            foreach (var civilianBuilding in levelInfo.CivilianBuildingGridPosisitions)
            {
                Vector2 centerPosition =
                    LevelGrid.Instance.GetCenterPositionFromCivilianBuilding(civilianBuilding.buildingId,
                        civilianBuilding.size);
                AllBuildingsPositions.Add(civilianBuilding.buildingId,centerPosition);
            }
        }
        
        public virtual List<BlockInfo> GetCivilianBuildingToBlock(IBuildingsSO constructionBuildingInfo)
        {
            List<BlockInfo> blockerList = new List<BlockInfo>();
            foreach (var ownedBuilding in AllBuildingsPositions)
            {
                int buildingSize = LevelGrid.Instance.GetSizeFromBuildingID(ownedBuilding.Key);
                
                if (CurrentBuildingsDictionary.ContainsKey(ownedBuilding.Key))
                {
                    BlockInfo blockInfo = new BlockInfo(ownedBuilding.Key, buildingSize,
                        AllBuildingsPositions[ownedBuilding.Key]);
                    blockerList.Add(blockInfo); 
                }
                else
                {
                    //Check if there is no space even if it is free
                    if (buildingSize < constructionBuildingInfo.buildSize)
                    {
                        BlockInfo blockInfo = new BlockInfo(ownedBuilding.Key, buildingSize,
                            AllBuildingsPositions[ownedBuilding.Key]);
                        blockerList.Add(blockInfo);
                    }
                }
            }
            return blockerList;
        }
        
        public virtual void DestroyBuilding(int locationID)
        {
            if (CurrentBuildingsDictionary.ContainsKey(locationID))
            {
                Destroy(CurrentBuildingsDictionary[locationID]);
                CurrentBuildingsDictionary.Remove(locationID);
            }
        }
        
        
    }
}