using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildingsTest;
using CDebugger;
using MainNavBarUI;
using TMPro;
using UnityEngine;

namespace Buildings.MilitaryBuildings
{
    public class MilitaryBuildingsUIManager : IBuildingsUIManager
    {
        public static MilitaryBuildingsUIManager Instance;

        public Action<IBuildingsSO> OnSpawnBlockers;
        public Action<IBuildingsSO> OnDespawnBlockers;

    
        
        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                CustomDebugger.LogError(LogCategories.MilitaryBuildings, "There is already an instance of MilitaryBuildingsUIManager");
            }
            Instance = this;
        
            _buildings = new List<IBuildingsSO>();
            _buildings = Resources.LoadAll<IBuildingsSO>("MilitaryBuildings").ToList();
            _BuildingContainersList = new List<IBuildingContainer>();
            buildingButtonText = buildButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            base.Start();
            MilitaryBuildingUIPanel.onMilitaryBuildingOpened += OnUIOpened;
            MilitaryBuildingUIPanel.onMilitaryBuildingClosed += OnUIClosed;
        }
        
        private void OnDisable()
        {
            MilitaryBuildingUIPanel.onMilitaryBuildingOpened -= OnUIOpened;
            MilitaryBuildingUIPanel.onMilitaryBuildingClosed -= OnUIClosed;
        }
        
        /// <summary>
        /// Method used to start building a civilian house in case of beign in popup, or start selection mode
        /// </summary>
        protected override void StartBuildingConstruction()
        {
            if (_currentContainerSelected == null || _currentSelectedBuilding == null)
                return;
        
            StartSelectionMode();
        }

        protected override void SpawnPanelContainers()
        {
            foreach (var building in _buildings)
            {
                var militaryBuilding = building as MilitaryBuildingsSO;
                GameObject newBuilding = Instantiate(BuildingContainerPrefab, Vector3.zero, Quaternion.identity,
                    gridContainerInCanvas.transform);
                MilitaryBuildingContainer container = newBuilding.GetComponent<MilitaryBuildingContainer>();
                container.SetProperties(militaryBuilding);
                _BuildingContainersList.Add(container);
            }
        }
        
        protected override void StartSelectionMode()
        {
            GridSlot closestAvailablePlaceForBuilding = LevelGrid.Instance.GetClosestAvailablePositionToMilitaryBuilding();
            if (closestAvailablePlaceForBuilding == null) //TODO KW: CHANGE FOR == NULL
            {
                CustomDebugger.Log(LogCategories.MilitaryBuildings, "No space for military buildings in map");
            }
            else
            {
                OnSpawnBlockers?.Invoke(_currentSelectedBuilding);

                var positionToBuild = closestAvailablePlaceForBuilding._gridPosition;
                //LevelGrid.Instance.SetCurrentGridSlotFromWorldPosition(positionToBuild);

                CameraScroll.Instance.CenterCameraOnBuilding(positionToBuild.y);
                playerIsTryingToStartConstruction = true;
                CustomDebugger.Log(LogCategories.MilitaryBuildings, "Default Place selected for military building" + closestAvailablePlaceForBuilding._gridPosition.ToString());

                OnChoosingBuildingPlace?.Invoke(_currentSelectedBuilding);
            }
            //Find civilian building closest to camera position
            NavigationManager.Instance.CloseCurrentTab();
            BuildingsUIPopButtons.Instance.CloseBuildUI();
        }
    }
}