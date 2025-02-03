using System;
using Buildings.CivilianBuildings.CivilianBuildingFSM;
using UnityEngine;
using UnityEngine.InputSystem.OSX;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuilding : MonoBehaviour
    {
        private CivilianBuildingState status;

        /// <summary>
        /// 0 - Undefined
        /// 1 - Start Building
        /// 2 - Builded
        /// 3 - Destroying
        /// </summary>
        private int buildingStatusFromLoad = 1;

        private void OnEnable()
        {
            StartBuildingBehaviour();
        }

        private void Update()
        {
            status.Run();
        }

        public void StartBuildingBehaviour()
        {
            switch (buildingStatusFromLoad)
            {
                case 0:
                    break;
                case 1:
                    //Spawn minions close to building
                    Debug.Log("FSM: SPAWN MINIONS");
                    TransitionToState(new ConstructingCivilianBuildingState(this));
                    break;
                case 2:
                    TransitionToState(new BuildedState(this));
                    break;
                case 3:
                    TransitionToState(new DestroyedState(this));
                    break;
            }
        }
        
        public void TransitionToState(CivilianBuildingState newState)
        {
            if(status != null)
                Debug.Log("FSM: TRANSITION FROM STATE: " + status.ToString() + " TO " + newState.ToString());
            this.status = newState;
            this.status.SetStatus(this);
        }
    }
}