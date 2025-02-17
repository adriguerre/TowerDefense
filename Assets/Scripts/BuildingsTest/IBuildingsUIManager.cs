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
        
        
        
        /// <summary>
        /// This is a helper method, used to get sprites from resources
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public Sprite GetSpriteFromResource(ResourceType resourceType)
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