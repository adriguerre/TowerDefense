using UnityEngine;

namespace Peasants.PeasantFSM
{
    public class Peasant_Building : PeasantState
    {
        public Peasant_Building(Peasant building) : base(building)
        {
            _peasant.StartBuildingAnimation();

        }

        public override void Execute()
        {
            Debug.Log("FSM:Running building state");
        }
    }
}