using System;
using System.Collections.Generic;
using System.Drawing;
using Buildings.MilitaryBuildings;
using BuildingsTest;
using CDebugger;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class ConstructionBuildBlocker : ISingleton<ConstructionBuildBlocker>
    {
        [SerializeField] private GameObject SingleBuildingBlockerSize1Prefab;
        [SerializeField] private GameObject CivilianBuildingBlockerSize4Prefab;
        [SerializeField] private GameObject CivilianBuildingBlockerSize6Prefab;

        private List<GameObject> civilianBuildingBlockers;
        private List<GameObject> militaryBuildingBlockers;
        

        private void Awake()
        {
            Debug.Log(CivilianBuildingsUIManager.Instance);
            CivilianBuildingsUIManager.Instance.OnSpawnBlockers += SpawnBlockersForCivilianBuildings;
            MilitaryBuildingsUIManager.Instance.OnSpawnBlockers += SpawnBlockersForMilitaryBuildings;
        }
        
        private void OnDestroy()
        {
            CivilianBuildingsUIManager.Instance.OnSpawnBlockers -= SpawnBlockersForCivilianBuildings;
            MilitaryBuildingsUIManager.Instance.OnSpawnBlockers -= SpawnBlockersForMilitaryBuildings;
        }

        private void Start()
        {
            civilianBuildingBlockers = new List<GameObject>();
            militaryBuildingBlockers = new List<GameObject>();
        }

        private void SpawnBlockersForCivilianBuildings(IBuildingsSO buildingInfo)
        {
            List<BlockInfo> blockersPositions = CivilianBuildingsManager.Instance.GetCivilianBuildingToBlock(buildingInfo);

            foreach (var blockPosition in blockersPositions)
            {
                int buildingSize =  LevelGrid.Instance.GetSizeFromBuildingID(buildingInfo.buildingID);
                if(blockPosition.buildingSize == 4)
                    civilianBuildingBlockers.Add(Instantiate(CivilianBuildingBlockerSize4Prefab,blockPosition.blockPosition, Quaternion.identity));
                else
                    civilianBuildingBlockers.Add(Instantiate(CivilianBuildingBlockerSize6Prefab, blockPosition.blockPosition, Quaternion.identity));
            }
        }
        private void SpawnBlockersForMilitaryBuildings(IBuildingsSO militaryBuildingInfo)
        {
            //We block civilian buildings as well
            SpawnBlockersForCivilianBuildings(militaryBuildingInfo);

            List<GridPosition> blockPositions = LevelGrid.Instance.GetAllPositionsToBlockWhenBuilding();
                
            foreach (var blockPosition in blockPositions)
            {
                civilianBuildingBlockers.Add(Instantiate(SingleBuildingBlockerSize1Prefab,LevelGrid.Instance.GetWorldPosition(blockPosition), Quaternion.identity));
            }   
        }
        
        
        public void DestroyCivilianBuildingsSpawnBlockers()
        {
            foreach (var blocker in civilianBuildingBlockers)
            {
                Destroy(blocker.gameObject);
            }
            civilianBuildingBlockers.Clear();
        }

        public void DestroyMilitaryBuildingsSpawnBlockers()
        {
            DestroyCivilianBuildingsSpawnBlockers();
            foreach (var blocker in militaryBuildingBlockers)
            {
                Destroy(blocker.gameObject);
            }
            militaryBuildingBlockers.Clear();
        }
    }


    [Serializable]
    public class BlockInfo
    {
        public int buildingID;
        public int buildingSize;
        public Vector2 blockPosition;

        public BlockInfo(int buildingID,int buildingSize, Vector2 blockPosition)
        {
            this.buildingSize = buildingSize;
            this.buildingID = buildingID;
            this.blockPosition = blockPosition;
        }
    }
}