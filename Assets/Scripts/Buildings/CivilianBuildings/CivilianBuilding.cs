using System;
using System.Collections.Generic;
using Buildings.CivilianBuildings.CivilianBuildingFSM;
using Game;
using UnityEngine;
using UnityEngine.InputSystem.OSX;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuilding : MonoBehaviour
    {
        private CivilianBuildingState status;
        private CivilianBuildingsSO _buildingSOInfo;
        private List<GameObject> builders;

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
                    
                    SpawnBuilders();
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

        private void SpawnBuilders()
        {
            Debug.Log("FSM: SPAWN MINIONS");
            Vector2 positionToSpawn = new Vector2(this.transform.position.x + InjectorManager.Instance.BuildersOffsetToBuilding.x,
                this.transform.position.y + InjectorManager.Instance.BuildersOffsetToBuilding.y);
            Debug.Log("POSITION: " + positionToSpawn);

            GameObject builderToSpawn = Instantiate(InjectorManager.Instance.BuilderPrefab, positionToSpawn,
                Quaternion.identity);

            builderToSpawn.transform.parent = this.transform;
        }

        public void SetReferences(CivilianBuildingsSO buildingSOInfo)
        {
            _buildingSOInfo = buildingSOInfo;
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