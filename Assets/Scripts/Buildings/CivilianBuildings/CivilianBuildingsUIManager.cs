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
    private List<CivilianBuildingContainer> _civilianBuildingContainersList;
    private List<CivilianBuildingsSO> _civilianBuildings;
    private CivilianBuildingsSO _currentSelectedCivilianBuilding;
    private CivilianBuildingContainer _currentContainerSelected;
    public bool isPanelOpenedFromPopup { get; private set; }
    public Action<CivilianBuildingsSO> OnBuildingStarted;
    public Action<CivilianBuildingsSO> OnChoosingBuildingPlace;

    /// <summary>
    /// Int -> Building size
    /// </summary>
    public Action<CivilianBuildingsSO> OnSpawnBlockers;
    public Action<CivilianBuildingsSO> OnDespawnBlockers;
    
    public bool playerIsChoosingPlaceToCivilianBuild { get; set; }
    
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
        _civilianBuildingContainersList = new List<CivilianBuildingContainer>();
        buildingButtonText = buildButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        CivilianBuildingUIPanel.onCivilianBuildingOpenedWithoutPopup += OnUIOpenedWithoutPopup;
        CivilianBuildingUIPanel.onCivilianBuildingOpened += OnUIOpened;
        CivilianBuildingUIPanel.onCivilianBuildingClosed += OnUIClosed;
        SpawnBuildingContainersInPanel();
        buildButton.onClick.AddListener(() => BuildCivilianBuilding());
    }

    private void OnDisable()
    {
        CivilianBuildingUIPanel.onCivilianBuildingOpenedWithoutPopup += OnUIOpenedWithoutPopup;
        CivilianBuildingUIPanel.onCivilianBuildingOpened -= OnUIOpened;
        CivilianBuildingUIPanel.onCivilianBuildingClosed -= OnUIClosed;
        
    }

    private void OnUIClosed()
    {
        StopRefreshingContainerStatus();
    }

    private void OnUIOpened()
    {
        StartRefreshingContainerStatus();
    }

    private void OnUIOpenedWithoutPopup()
    {
        UnblockAllBuildingsInUIPanel();
    }


    /// <summary>
    /// Method used to start building a civilian house in case of beign in popup, or start selection mode
    /// </summary>
    private void BuildCivilianBuilding()
    {
        if (_currentContainerSelected == null || _currentSelectedCivilianBuilding == null)
            return;
        
        //If we are building from popup, we want to insta build it where we select in the map
        if (isPanelOpenedFromPopup)
        {
            BuildFromPopupPanel();
        }
        //Otherwise, we will want to select where we want to build this, we will make visible all posible locations and make player select one
        else
        {
            StartSelectionMode();
        }
    }

    private void StartSelectionMode()
    {
        //Block all buildings slots that are not available
        //START SELECTION MODE
        CivilianBuildingGridPosition closestBuilding = LevelGrid.Instance.GetClosestCivilianBuildingToMousePositionAndActivateGrid(_currentSelectedCivilianBuilding.buildSize);
        if (closestBuilding == null)
        {
            Debug.Log("NO AVAILABLE CIVILIAN BUILDING");
        }
        else
        {
            OnSpawnBlockers?.Invoke(_currentSelectedCivilianBuilding);

            var positionToBuild = LevelGrid.Instance.GetCenterPositionFromCivilianBuilding(closestBuilding.buildingId, _currentSelectedCivilianBuilding.buildSize);
            LevelGrid.Instance.SetCurrentGridSlotFromWorldPosition(positionToBuild);

            CameraScroll.Instance.CenterCameraOnBuilding(positionToBuild.y);
            playerIsChoosingPlaceToCivilianBuild = true;
            Debug.Log("KWB: " + positionToBuild);
            OnChoosingBuildingPlace?.Invoke(_currentSelectedCivilianBuilding);
        }
        //Find civilian building closest to camera position
        NavigationManager.Instance.CloseCurrentTab();
        CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
    }

    private void BuildFromPopupPanel()
    {
        //TODO KW: Check resources
        Debug.Log("KW: BUYING CIVILIAN BUILDING");
        //TODO KW: Cambiar el modo de pasar argumentos a ese método para no tener que buscar todo el rato ( si es madera pasa x, si es lo otro pasa y)
        //TODO if(InjectorManager.Instance.TryToSpendResources())
        OnBuildingStarted?.Invoke(_currentSelectedCivilianBuilding);
        NavigationManager.Instance.CloseCurrentTab();
        CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
        isPanelOpenedFromPopup = false;
    }

    /// <summary>
    /// Spawn containers in civilianbuilding UI panel
    /// </summary>
    private void SpawnBuildingContainersInPanel()
    {
        foreach (var building in _civilianBuildings)
        {
            GameObject newBuilding = Instantiate(civilianBuildingContainerPrefab, Vector3.zero, Quaternion.identity,
                gridContainerInCanvas.transform);
            CivilianBuildingContainer container = newBuilding.GetComponent<CivilianBuildingContainer>();
            container.SetProperties(building);
            _civilianBuildingContainersList.Add(container);
        }
    }
    
    /// <summary>
    /// When we select a possible location and open panel coming from popup, we will block buildings that are not eligible for build because of size
    /// </summary>
    /// <param name="size"></param>
    public void BlockBuildingsWithLargerSizeInUIPanel(int size)
    {
        isPanelOpenedFromPopup = true;
        foreach (var containers in _civilianBuildingContainersList)
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
    public void UnblockAllBuildingsInUIPanel()
    {
        isPanelOpenedFromPopup = false;
        foreach (var containers in _civilianBuildingContainersList)
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
    /// Methods to update text / container if there aren't enough resources
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void StartRefreshingContainerStatus()
    {
        foreach (var container in _civilianBuildingContainersList)
        {
            container.StartRefreshingIfPlayerHasResources();
        }
    }
    
    /// <summary>
    /// Methods to update text / container if there aren't enough resources
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void StopRefreshingContainerStatus()
    {
        foreach (var container in _civilianBuildingContainersList)
        {
            container.StopRefreshingIfPlayerHasResources();
        }
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