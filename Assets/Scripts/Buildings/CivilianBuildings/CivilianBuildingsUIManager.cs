using System;
using System.Collections.Generic;
using System.Linq;
using GameResources;
using MainNavBarUI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// This script is responsible of all the UI from the panel CivilianBuildings
/// </summary>
public class CivilianBuildingsUIManager : ISingleton<CivilianBuildingsUIManager>
{
    [SerializeField] private GameObject civilianBuildingContainerPrefab;
    [SerializeField] private Button buildButton;
    private TextMeshProUGUI buildingButtonText;
    [SerializeField] private GameObject gridContainerInCanvas;
    private List<CivilianBuildingContainer> civilianBuildingContainersList;
    private List<CivilianBuildingsSO> _civilianBuildings;
    private CivilianBuildingsSO _currentSelectedCivilianBuilding;
    private CivilianBuildingContainer _currentContainerSelected;
    public bool isPanelOpenedFromPopup { get; private set; }
    public Action<CivilianBuildingsSO> OnBuildingStarted;
    public Action<CivilianBuildingsSO> OnChoosingBuildingPlace;
    
    public bool playerIsChoosingPlaceToBuild { get; set; }
    
    [Header("Resources Sprites")] 
    [SerializeField] private Sprite foodSprite;
    [SerializeField] private Sprite woodSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite goldSprite;
    protected override void Awake()
    {
        base.Awake();
        _civilianBuildings = new List<CivilianBuildingsSO>();
        _civilianBuildings = Resources.LoadAll<CivilianBuildingsSO>("CivilianBuildings").ToList();
        civilianBuildingContainersList = new List<CivilianBuildingContainer>();
        buildingButtonText = buildButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        CivilianBuildingUIPanel.onCivilianBuildingOpenedWithoutPopup += onUIOpenedWithoutPopup;
        SpawnBuildingContainers();
        buildButton.onClick.AddListener(() => BuildCivilianBuilding());
    }

    private void onUIOpenedWithoutPopup()
    {
        UnblockAllBuildings();
    }

    /// <summary>
    /// Method used to start building a civilian house
    /// </summary>
    private void BuildCivilianBuilding()
    {
        
        if (_currentContainerSelected == null || _currentSelectedCivilianBuilding == null)
            return;
            
            
        //If we are building from popup, we want to insta build it where we select in the map
        if (isPanelOpenedFromPopup)
        {
            //TODO KW: Check resources
            Debug.Log("KW: BUYING CIVILIAN BUILDING");
            //TODO KW: Cambiar el modo de pasar argumentos a ese método para no tener que buscar todo el rato ( si es madera pasa x, si es lo otro pasa y)
            //if(ResourcesManager.Instance.TryToSpendResources())
            OnBuildingStarted?.Invoke(_currentSelectedCivilianBuilding);
            LevelGrid.Instance.currentGridSlot.AddCivilianBuildingToAllSlot(_currentSelectedCivilianBuilding);
            NavigationManager.Instance.CloseCurrentTab();
            CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
            isPanelOpenedFromPopup = false;
        }
        //Otherwise, we will want to select where we want to build this, we will make visible all posible locations and make player select one
        else
        {

            CivilianBuildingGridPosition closestBuilding = LevelGrid.Instance.GetClosestCivilianBuildingToMousePosition(_currentSelectedCivilianBuilding.buildSize);
           
            if (closestBuilding == null)
            {
                Debug.Log("NO AVAILABLE CIVILIAN BUILDING");
            }
            else
            {
                var positionToBuild = LevelGrid.Instance.GetCenterPositionFromCivilianBuilding(closestBuilding.buildingId, 6);
                CameraScroll.Instance.CenterCameraOnBuilding(positionToBuild.y);
                playerIsChoosingPlaceToBuild = true;
                Debug.Log("CLOSEST CIVILIAN BUILDING AVAILABLE: " + closestBuilding.buildingId);
                OnChoosingBuildingPlace?.Invoke(_currentSelectedCivilianBuilding);
            }
            //Find civilian building closest to camera position
            NavigationManager.Instance.CloseCurrentTab();
            CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
        }
    }

    /// <summary>
    /// Spawn containers in civilianbuilding UI panel
    /// </summary>
    private void SpawnBuildingContainers()
    {
        foreach (var building in _civilianBuildings)
        {
            GameObject newBuilding = Instantiate(civilianBuildingContainerPrefab, Vector3.zero, Quaternion.identity,
                gridContainerInCanvas.transform);
            CivilianBuildingContainer container = newBuilding.GetComponent<CivilianBuildingContainer>();
            container.SetProperties(building);
            civilianBuildingContainersList.Add(container);
        }
    }
    
    /// <summary>
    /// When we select a possible location and open panel coming from popup, we will block buildings that are not eligible for build because of size
    /// </summary>
    /// <param name="size"></param>
    public void BlockBuildingsWithLargerSize(int size)
    {
        isPanelOpenedFromPopup = true;
        foreach (var containers in civilianBuildingContainersList)
        {
            if (containers._civilianBuildingInfo.buildSize > size)
            {
                containers.BlockContainer();
            }
            else
            {
                containers.UnblockContainer();
            }
        }
    }
    
    /// <summary>
    /// If we open normal civilianUIPanel, we will unblock all possible buildings
    /// </summary>
    public void UnblockAllBuildings()
    {
        isPanelOpenedFromPopup = false;
        foreach (var containers in civilianBuildingContainersList)
        {
            containers.UnblockContainer();
        }
    }

    /// <summary>
    /// Select specific building, showing info in button and activating selector game object
    /// </summary>
    /// <param name="ContainerSelected"></param>
    /// <param name="civilianBuilding"></param>
    public void SelectBuildingInMenu(CivilianBuildingContainer ContainerSelected, CivilianBuildingsSO civilianBuilding)
    {
        if (civilianBuilding == _currentSelectedCivilianBuilding)
            return;
        
        _currentSelectedCivilianBuilding = civilianBuilding;
        if (_currentContainerSelected != null)
        {
            _currentContainerSelected.HideSelectorUI();
        }
        _currentContainerSelected = ContainerSelected;
        buildingButtonText.text = "Build " + _currentSelectedCivilianBuilding.buildingName;

    }

    /// <summary>
    /// Unselect container, used when we close the panel, to reset all posible values/problems
    /// </summary>
    public void UnSelectBuildingInMenu()
    {
        _currentSelectedCivilianBuilding = null;
        if (_currentContainerSelected != null)
        {
            _currentContainerSelected.HideSelectorUI();
        }
        _currentContainerSelected = null;
        buildingButtonText.text = "Build";
    }

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