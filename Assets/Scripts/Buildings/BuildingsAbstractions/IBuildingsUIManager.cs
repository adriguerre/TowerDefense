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
        protected List<CivilianBuildingContainer> _BuildingContainersList;
        protected List<IBuildingsSO> _buildings;
        protected IBuildingsSO _currentSelectedBuilding;
        
        public Action<IBuildingsSO> OnChoosingBuildingPlace;

        [Header("Resources Sprites")] 
        [SerializeField] protected Sprite foodSprite;
        [SerializeField] protected Sprite woodSprite;
        [SerializeField] protected Sprite stoneSprite;
        [SerializeField] protected Sprite ironSprite;
        [SerializeField] protected Sprite goldSprite;

        protected abstract void StartSelectionMode();

        
        /// <summary>
        /// Select specific building, showing info in button and activating selector game object
        /// </summary>
        /// <param name="ContainerSelected"></param>
        /// <param name="civilianBuilding"></param>
        public virtual void SelectBuildingInMenu(CivilianBuildingContainer ContainerSelected, IBuildingsSO civilianBuilding)
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
        public virtual void UnSelectBuildingInMenu()
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
        protected virtual void StartRefreshingContainerStatus()
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
        protected virtual void StopRefreshingContainerStatus()
        {
            foreach (var container in _BuildingContainersList)
            {
                container.StopRefreshingIfPlayerHasResources();
            }
        }
        
        /// <summary>
        /// This is a helper method, used to get sprites from resources
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public virtual Sprite GetSpriteFromResource(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Undefined:
                    break; 
                case ResourceType.Food:
                    return foodSprite;
                    break; 
                case ResourceType.Wood:
                    return woodSprite;
                    break; 
                case ResourceType.Stone:
                    return stoneSprite;
                    break; 
                case ResourceType.Iron:
                    return ironSprite;
                    break; 
                case ResourceType.Gold:
                    return goldSprite;
                    break; 
            }
            return null;
        }
    }
}