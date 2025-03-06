using System;
using System.Collections.Generic;
using Buildings.CivilianBuildings.CivilianBuildingFSM;
using BuildingsTest;
using Game;
using Peasants;
using Peasants.PeasantFSM;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuilding : MonoBehaviour
    {
        public IBuildingsSO BuildingSOInfo { get; private set; }

        
        private CivilianBuildingState _buildingStatus;
        private Peasant builder;
        private BuildingFillAmount _buildingFiller;
        /// <summary>
        /// 0 - Undefined
        /// 1 - Start Building
        /// 2 - Builded
        /// 3 - Destroying
        /// </summary>
        private int buildingStatusFromLoad = 1;
        
        
        [field: SerializeField] public GameObject BuildedGameObject { get; private set; }
        [field: SerializeField] public GameObject ConstructionGameObject { get; private set; }

        
        private void Update()
        {
            if(_buildingStatus != null)
                _buildingStatus.Execute();
        }

        public void StartBuildingBehaviour()
        {
            switch (buildingStatusFromLoad)
            {
                case 0:
                    break;
                case 1:
                    //Spawn minions close to building
                    
                    //SpawnBuilders();
                    MoveBuilderToLocation();
                    SpawnConstructionUISlider();
                    TransitionToState(new ConstructingCivilianBuildingState(this));
                    break;
                case 2:
                    if(BuildingSOInfo is CivilianBuildingsSO)
                        TransitionToState(new CivilianBuildedState(this));
                    else
                        Debug.Log("ES");
                    break;
                case 3:
                    TransitionToState(new DestroyedState(this));
                    break;
            }
        }
        
        private void SpawnConstructionUISlider()
        {
            GameObject constructionPercentage = gameObject.transform.Find("ConstructionPercentage").gameObject;
            _buildingFiller = constructionPercentage.GetComponentInChildren<BuildingFillAmount>();
            _buildingFiller.StartFilling(0, BuildingSOInfo.timeToBuild);
            _buildingFiller.onBuildingFinished += OnBuildingFinished;
        }

        private void OnBuildingFinished(object sender, EventArgs e)
        {
            Debug.Log("SE HA TERMINADO EL EDIFICIO " + sender.ToString());
            //Remove fill amount UI
            BuildingFillAmount buildingFiller = sender as BuildingFillAmount;
            buildingFiller.onBuildingFinished -= OnBuildingFinished;
            
            //Transition from Builded -> Start production
            if(BuildingSOInfo is CivilianBuildingsSO)
                TransitionToState(new CivilianBuildedState(this));
           
            //GetComponent<SpriteRenderer>().sprite = buildedSprite;

            //Peasant [Builder] transition to moving away
            
            //Calculate position to move away
            if (builder != null)
            {
                ChangePeasantBehaviourOnBuildedFinished();
            }
        }

        public void StopBuilderFromBuilding()
        {
            if (builder != null)
            {
                ChangePeasantBehaviourOnBuildedFinished();
                builder = null;
            }
        }

        private void ChangePeasantBehaviourOnBuildedFinished()
        {
            Vector2 positionToMoveBuilder = new Vector2(builder.transform.position.x - InjectorManager.Instance.BuildersOffsetToMoveAwayFromBuilding.x,
                builder.transform.position.y - InjectorManager.Instance.BuildersOffsetToMoveAwayFromBuilding.y);
            builder.TransitionToState(new Peasant_Move(builder, positionToMoveBuilder, true));
            builder = null;
        }

        private void MoveBuilderToLocation()
        {
            Vector2 positionToSpawn = new Vector2(this.transform.position.x + InjectorManager.Instance.BuildersOffsetToBuilding.x,
                this.transform.position.y + InjectorManager.Instance.BuildersOffsetToBuilding.y);
            
            Peasant peasant = PeasantsManager.Instance.GetClosestPeasantToPosition(positionToSpawn);

            if (peasant != null)
            {
                peasant.TransitionToState(new Peasant_Move(peasant, positionToSpawn, false));
                builder = peasant;
            }
        }

        private void OnDestroy()
        {
            StopBuilderFromBuilding();
        }
        // private void SpawnBuilders()
        // {
        //     Vector2 positionToSpawn = new Vector2(this.transform.position.x + InjectorManager.Instance.BuildersOffsetToBuilding.x,
        //         this.transform.position.y + InjectorManager.Instance.BuildersOffsetToBuilding.y);
        //
        //     GameObject builderToSpawn = Instantiate(InjectorManager.Instance.BuilderPrefab, positionToSpawn,
        //         Quaternion.identity);
        //     builderToSpawn.transform.parent = this.transform;
        //     builder =builderToSpawn;
        // }

        public void Init(IBuildingsSO buildingSOInfo)
        {
            BuildingSOInfo = buildingSOInfo;
            StartBuildingBehaviour();
        }
        
        public void TransitionToState(CivilianBuildingState newState)
        {
            if(_buildingStatus != null)
                Debug.Log("FSM: TRANSITION FROM STATE: " + _buildingStatus.ToString() + " TO " + newState.ToString());
            this._buildingStatus = newState;
            this._buildingStatus.SetReference(this);
        }
    }
}