﻿using UnityEngine;

namespace Buildings.CivilianBuildings.CivilianBuildingFSM
{
    public class DestroyedState : CivilianBuildingState
    {
        public DestroyedState(CivilianBuilding building) : base(building)
        {
        }

        public override void Execute()
        {
            Debug.Log("FSM: Running destroyed state");
        }
    }
}