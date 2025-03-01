using System;
using System.Collections.Generic;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingsTest
{
    public abstract class IBuildingsUIManager : MonoBehaviour
    {
        [SerializeField] protected GameObject BuildingContainerPrefab;
        [SerializeField] protected Button buildButton;
        protected TextMeshProUGUI buildingButtonText;
        [SerializeField] protected GameObject gridContainerInCanvas;
        
        protected IBuildingContainer _currentContainerSelected;
        protected List<IBuildingContainer> _BuildingContainersList;
        protected List<IBuildingsSO> _buildings;
        protected IBuildingsSO _currentSelectedBuilding;
        
        public Action<IBuildingsSO> OnChoosingBuildingPlace;
        public bool playerIsTryingToStartConstruction;

        protected abstract void StartSelectionMode();
        /// <summary>
        /// Start selection mode or start building (if it is coming from popup)
        /// </summary>
        protected abstract void StartBuildingConstruction();
        /// <summary>
        /// Spawn container inside ui to show info
        /// </summary>
        protected abstract void SpawnPanelContainers();

        protected virtual void Start()
        {
           // SpawnPanelContainers();
           // buildButton.onClick.AddListener(() => StartBuildingConstruction());
        }
        
        /// <summary>
        /// Select specific building, showing info in button and activating selector game object
        /// </summary>
        /// <param name="ContainerSelected"></param>
        /// <param name="civilianBuilding"></param>
        public void SelectBuildingInMenu(IBuildingContainer ContainerSelected, IBuildingsSO civilianBuilding)
        {
            if (civilianBuilding == _currentSelectedBuilding)
                return;
            
            _currentSelectedBuilding = civilianBuilding;
            if (_currentContainerSelected != null)
            {
                _currentContainerSelected.HideSelectorUI();
            }
            _currentContainerSelected = ContainerSelected;
            buildingButtonText.text = "Build " + _currentSelectedBuilding.buildingName;
    
        }
    
        /// <summary>
        /// Unselect container, used when we close the panel, to reset all posible values/problems
        /// </summary>
        public void UnSelectBuildingInMenu()
        {
            _currentSelectedBuilding = null;
            if (_currentContainerSelected != null)
            {
                _currentContainerSelected.HideSelectorUI();
            }
            _currentContainerSelected = null;
            buildingButtonText.text = "Build";
        }
        
        
        /// <summary>
        /// Methods to update text / container if there aren't enough resources
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected void StartRefreshingContainerStatus()
        {
            foreach (var container in _BuildingContainersList)
            {
                container.StartRefreshingIfPlayerHasResources();
            }
        }
        
        /// <summary>
        /// Methods to update text / container if there aren't enough resources
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected void StopRefreshingContainerStatus()
        {
            foreach (var container in _BuildingContainersList)
            {
                container.StopRefreshingIfPlayerHasResources();
            }
        }
        
        protected void OnUIClosed()
        {
            StopRefreshingContainerStatus();
        }

        protected void OnUIOpened()
        {
            StartRefreshingContainerStatus();
        }

    }
}