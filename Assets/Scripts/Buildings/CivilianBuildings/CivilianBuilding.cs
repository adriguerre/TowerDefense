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
        private GameObject builder;
        private CivilianBuildingFillAmount _buildingFiller;

        [SerializeField] private Sprite buildedSprite;

        /// <summary>
        /// 0 - Undefined
        /// 1 - Start Building
        /// 2 - Builded
        /// 3 - Destroying
        /// </summary>
        private int buildingStatusFromLoad = 1;

        private void OnEnable()
        {
            
        }

        private void Update()
        {
            if(status != null)
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
                    SpawnConstructionUISlider();
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

        private void SpawnConstructionUISlider()
        {
            GameObject constructionPercentage = gameObject.transform.Find("ConstructionPercentage").gameObject;
            _buildingFiller = constructionPercentage.GetComponentInChildren<CivilianBuildingFillAmount>();
            _buildingFiller.StartFilling(0, _buildingSOInfo.timeToBuild);
            _buildingFiller.onBuildingFinished += OnBuildingFinished;
        }

        private void OnBuildingFinished(object sender, EventArgs e)
        {
            Debug.Log("SE HA TERMINADO EL EDIFICIO " + sender.ToString());
            CivilianBuildingFillAmount buildingFiller = sender as CivilianBuildingFillAmount;
            buildingFiller.onBuildingFinished -= OnBuildingFinished;
            TransitionToState(new BuildedState(this));
            GetComponent<SpriteRenderer>().sprite = buildedSprite;
        }


        private void SpawnBuilders()
        {
            Vector2 positionToSpawn = new Vector2(this.transform.position.x + InjectorManager.Instance.BuildersOffsetToBuilding.x,
                this.transform.position.y + InjectorManager.Instance.BuildersOffsetToBuilding.y);

            GameObject builderToSpawn = Instantiate(InjectorManager.Instance.BuilderPrefab, positionToSpawn,
                Quaternion.identity);

            builderToSpawn.transform.parent = this.transform;
            builder =builderToSpawn;
        }

        public void Init(CivilianBuildingsSO buildingSOInfo)
        {
            _buildingSOInfo = buildingSOInfo;
            StartBuildingBehaviour();
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