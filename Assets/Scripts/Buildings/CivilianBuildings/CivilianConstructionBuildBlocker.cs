using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class CivilianConstructionBuildBlocker : MonoBehaviour
    {
        [SerializeField] private GameObject CivilianBuildingBlockerSize4Prefab;
        [SerializeField] private GameObject CivilianBuildingBlockerSize6Prefab;

        private List<GameObject> civilianBuildingBlockers;
        

        private void Awake()
        {
            CivilianBuildingsUIManager.Instance.OnSpawnBlockers += SpawnBlockers;
        }

        private void Start()
        {
            civilianBuildingBlockers = new List<GameObject>();
        }

        private void SpawnBlockers(CivilianBuildingsSO buildingInfo)
        {
            List<BlockInfo> blockersPositions = CivilianBuildingsManager.Instance.GetCivilianBuildingToBlock();

            foreach (var blockPosition in blockersPositions)
            {
                int buildingSize =  LevelGrid.Instance.GetSizeFromBuildingID(buildingInfo.buildingID);
                if(blockPosition.buildingSize == 4)
                    civilianBuildingBlockers.Add(Instantiate(CivilianBuildingBlockerSize4Prefab,blockPosition.blockPosition, Quaternion.identity));
                else
                    civilianBuildingBlockers.Add(Instantiate(CivilianBuildingBlockerSize6Prefab, blockPosition.blockPosition, Quaternion.identity));

            }
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