using System.Collections;
using GameResources;
using UnityEngine;

namespace Buildings.CivilianBuildings.CivilianBuildingFSM
{
    public class BuildedState : CivilianBuildingState
    {
        public BuildedState(CivilianBuilding building) : base(building)
        {
            building.BuildedGameObject.SetActive(true);
            building.ConstructionGameObject.SetActive(false);
            CivilianBuildingsSO buildingSOInfo = building.BuildingSOInfo;
            Debug.Log("KW: " + buildingSOInfo);
            ResourcesManager.Instance.IncreaseResourceProduction(buildingSOInfo.resourceProduced.resourceProduced, buildingSOInfo.resourceProduced.resourceProducedBaseLevel1);
        }

        public override void Execute()
        {
            Debug.Log("FSM:Running builded state");
        }
        
        
    }
}