using UnityEngine;

namespace Peasants.PeasantFSM
{
    public class Peasant_LeavingBuilding : PeasantState
    {
        public Peasant_LeavingBuilding(Peasant peasant, Vector2 location) : base(peasant)
        {
            _peasant.StopBuildingAnimation();
        }

        public override void Execute()
        {
            Debug.Log("Peasant_LeavingBuilding is executing");
        }
    }
}