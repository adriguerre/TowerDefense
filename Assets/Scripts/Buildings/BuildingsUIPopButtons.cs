using System;
using CDebugger;
using MainNavBarUI;
using PopupSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Pop up that shows when players click on a specific civilian building, allowing them to upgrade/build/Destroy, depending on the build status
/// </summary>
public class BuildingsUIPopButtons : ISingleton<BuildingsUIPopButtons>
{
    private GameObject _buildingUIPopup;
    private Animator _animator;
    public Action onCameraCenterCompleted;
    private Vector2 buildingInPosition;
    private int buildingSize;
    private GridSlot popupOnGridSlot;
    [SerializeField] private Button buildButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button destroyButton;
    protected override void Awake()
    {
        _buildingUIPopup = this.gameObject.transform.Find("BuildingsBuildUIPopup").gameObject;
        _buildingUIPopup.SetActive(false);
        onCameraCenterCompleted += OnCameraCenterCompleted;
        _animator = GetComponent<Animator>();
    }
    
    private void OnPanelClick()
    { 
        CloseBuildUI();
    }

    public void OpenBuildUI(Vector2 position, GridSlot gridSlot)
    {
        CustomDebugger.Log(LogCategories.Buildings, "Opened Building Popup view");
        buildingSize = gridSlot.buildingSize;
        buildingInPosition = position;
        popupOnGridSlot = gridSlot;
        CameraScroll.Instance.SetIfPlayerCanMoveCamera(false);
        CameraScroll.Instance.CenterCameraOnBuildingWithCallback(position.y, onCameraCenterCompleted);

        buildButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        destroyButton.onClick.RemoveAllListeners();
        
    }

    private void HandleButtonsVisibility(GridSlot gridSlot)
    {
        if (gridSlot.GetBuildingInGridSlot() == null)
        {
            //Solo se puede construir, no upgradear ni destruir
            buildButton.onClick.AddListener(() => OpenCivilianBuildingsUIWithPopup(buildingInPosition));
            buildButton.interactable = true;
            upgradeButton.interactable = false;
            destroyButton.interactable = false;
        }
        else
        {
            destroyButton.onClick.AddListener(() => DestroyBuilding(buildingInPosition));
            upgradeButton.onClick.AddListener(() => UpgradeBuildingPopUp(buildingInPosition));
            buildButton.interactable = false; 
            upgradeButton.interactable = true;
            destroyButton.interactable = true;
        }
    }

    private void DestroyBuilding(Vector2 vector2)
    {
        GridSlot buildingGridSlot = popupOnGridSlot;
        PopupManager.Instance.ShowDestroyBuildingPopup(buildingGridSlot);
    }

    private void UpgradeBuildingPopUp(Vector2 vector2)
    {
        
    } 

    private void OpenCivilianBuildingsUIWithPopup(Vector2 vector2)
    {
       NavigationManager.Instance.OpenScreenCanvas(TabTypes.CivilianBuildings, true);
       CivilianBuildingsUIManager.Instance.BlockBuildingsWithLargerSizeInUIPanel(buildingSize);
    }

    private void OnCameraCenterCompleted()
    {
        _buildingUIPopup.SetActive(true);
        CivilianBuildingUIBlocker.onPanelClick += OnPanelClick;
        HandleButtonsVisibility(popupOnGridSlot);

        _animator.SetTrigger("onEnable");
        _buildingUIPopup.transform.position = Camera.main.WorldToScreenPoint(buildingInPosition);
    }

    public void CloseBuildUI()
    {
        if (_buildingUIPopup.activeSelf)
        {
            _animator.SetTrigger("onDisable");
        }
            
    }

    public void DisableCivilianBuildUI()
    {
        CivilianBuildingUIBlocker.onPanelClick -= OnPanelClick;
        _buildingUIPopup.SetActive(false);
        LevelGrid.Instance.DestroyGridBuildPrefab();
        // buildingSize = 0;
        CameraScroll.Instance.SetIfPlayerCanMoveCamera(true);
    }


}