﻿using UnityEngine;

namespace Buildings.CivilianBuildings.CivilianBuildingFSM
{
    public abstract class CivilianBuildingState
    {
        protected CivilianBuilding building;
        
        protected CivilianBuildingState(CivilianBuilding building)
        {
            this.building = building;
        }
        

        public void SetStatus(CivilianBuilding newStatus)
        {
            this.building = newStatus;
        }

        public abstract void Run();
    }
}