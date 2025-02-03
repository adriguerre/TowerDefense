using UnityEngine;

namespace Buildings.CivilianBuildings.CivilianBuildingFSM
{
    public class ConstructingCivilianBuildingState : CivilianBuildingState
    {
        public ConstructingCivilianBuildingState(CivilianBuilding building) : base(building)
        {
        }

        public override void Run()
        {
            Debug.Log("FSM: Running constructing state");
        }
    }
}