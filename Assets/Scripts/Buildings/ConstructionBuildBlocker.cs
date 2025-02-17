using System;
using System.Collections.Generic;
using System.Drawing;
using BuildingsTest;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class ConstructionBuildBlocker : ISingleton<ConstructionBuildBlocker>
    {
        [SerializeField] private GameObject CivilianBuildingBlockerSize4Prefab;
        [SerializeField] private GameObject CivilianBuildingBlockerSize6Prefab;

        private List<GameObject> civilianBuildingBlockers;
        

        private void Awake()
        {
            Debug.Log(CivilianBuildingsUIManager.Instance);
            CivilianBuildingsUIManager.Instance.OnSpawnBlockers += SpawnBlockersForCivilianBuildings;
        }

        private void OnDestroy()
        {
            CivilianBuildingsUIManager.Instance.OnSpawnBlockers -= SpawnBlockersForCivilianBuildings;

        }

        private void Start()
        {
            civilianBuildingBlockers = new List<GameObject>();
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

        public void DestroyCivilianBuildingsSpawnBlockers()
        {
            foreach (var blocker in civilianBuildingBlockers)
            {
                Destroy(blocker.gameObject);
            }
            civilianBuildingBlockers.Clear();
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