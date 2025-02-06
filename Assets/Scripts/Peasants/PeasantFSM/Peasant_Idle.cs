using UnityEngine;

namespace Peasants.PeasantFSM
{
    public class Peasant_Idle : PeasantState
    {
        public Peasant_Idle(Peasant building) : base(building)
        {
        }

        public override void Execute()
        {
            Debug.Log("FSM:Running idle state");
        }
    }
}