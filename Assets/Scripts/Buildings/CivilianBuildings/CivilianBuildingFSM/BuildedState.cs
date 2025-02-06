using UnityEngine;

namespace Buildings.CivilianBuildings.CivilianBuildingFSM
{
    public class BuildedState : CivilianBuildingState
    {
        public BuildedState(CivilianBuilding building) : base(building)
        {
        }

        public override void Execute()
        {
            Debug.Log("FSM:Running builded state");
        }
    }
}