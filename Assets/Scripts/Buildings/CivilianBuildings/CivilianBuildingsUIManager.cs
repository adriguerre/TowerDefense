using System;
using System.Collections.Generic;
using System.Linq;
using BuildingsTest;
using GameResources;
using MainNavBarUI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// This script is responsible of all the UI from the panel CivilianBuildings
/// </summary>
public class CivilianBuildingsUIManager : IBuildingsUIManager
{

    public static CivilianBuildingsUIManager Instance;
    
    public bool isPanelOpenedFromPopup { get; private set; }
    public Action<IBuildingsSO> OnBuildingStarted;

    /// <summary>
    /// Int -> Building size
    /// </summary>
    public Action<IBuildingsSO> OnSpawnBlockers;
    public Action<IBuildingsSO> OnDespawnBlockers;
    
    public bool playerIsChoosingPlaceToCivilianBuild { get; set; }
    

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("THERE IS ALREADY A CIV UI MANAGER");
        }

        Instance = this;
        
        _buildings = new List<IBuildingsSO>();
        _buildings = Resources.LoadAll<IBuildingsSO>("CivilianBuildings").ToList();
        _BuildingContainersList = new List<CivilianBuildingContainer>();
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
        if (_currentContainerSelected == null || _currentSelectedBuilding == null)
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

    protected override void StartSelectionMode()
    {
        //Block all buildings slots that are not available
        //START SELECTION MODE
        CivilianBuildingGridPosition closestBuilding = LevelGrid.Instance.GetClosestCivilianBuildingToMousePositionAndActivateGrid(_currentSelectedBuilding.buildSize);
        if (closestBuilding == null)
        {
            Debug.Log("NO AVAILABLE CIVILIAN BUILDING");
        }
        else
        {
            OnSpawnBlockers?.Invoke(_currentSelectedBuilding);

            var positionToBuild = LevelGrid.Instance.GetCenterPositionFromCivilianBuilding(closestBuilding.buildingId, _currentSelectedBuilding.buildSize);
            LevelGrid.Instance.SetCurrentGridSlotFromWorldPosition(positionToBuild);

            CameraScroll.Instance.CenterCameraOnBuilding(positionToBuild.y);
            playerIsChoosingPlaceToCivilianBuild = true;
            Debug.Log("KWB: " + positionToBuild);
            OnChoosingBuildingPlace?.Invoke(_currentSelectedBuilding);
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
        OnBuildingStarted?.Invoke(_currentSelectedBuilding);
        NavigationManager.Instance.CloseCurrentTab();
        CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
        isPanelOpenedFromPopup = false;
    }

    /// <summary>
    /// Spawn containers in civilianbuilding UI panel
    /// </summary>
    private void SpawnBuildingContainersInPanel()
    {
        foreach (var building in _buildings)
        {
            GameObject newBuilding = Instantiate(BuildingContainerPrefab, Vector3.zero, Quaternion.identity,
                gridContainerInCanvas.transform);
            CivilianBuildingContainer container = newBuilding.GetComponent<CivilianBuildingContainer>();
            container.SetProperties(building);
            _BuildingContainersList.Add(container);
        }
    }
    
    /// <summary>
    /// When we select a possible location and open panel coming from popup, we will block buildings that are not eligible for build because of size
    /// </summary>
    /// <param name="size"></param>
    public void BlockBuildingsWithLargerSizeInUIPanel(int size)
    {
        isPanelOpenedFromPopup = true;
        foreach (var containers in _BuildingContainersList)
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
        foreach (var containers in _BuildingContainersList)
        {
            containers.UnblockContainer();
        }
    }
    


}