using System;
using MainNavBarUI;
using PopupSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Pop up that shows when players click on a specific civilian building, allowing them to upgrade/build/Destroy, depending on the build status
/// </summary>
public class CivilianBuildingsUIPopButtons : ISingleton<CivilianBuildingsUIPopButtons>
{
    private GameObject civilianBuildUI;
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
        civilianBuildUI = this.gameObject.transform.Find("CivilianBuildingsBuildUI").gameObject;
        civilianBuildUI.SetActive(false);
        onCameraCenterCompleted += OnCameraCenterCompleted;
        _animator = GetComponent<Animator>();
    }
    
    private void OnPanelClick()
    { 
        CloseBuildUI();
    }

    public void OpenBuildUI(Vector2 position, GridSlot gridSlot)
    {
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
            destroyButton.onClick.AddListener(() => DestroyCivilianBuilding(buildingInPosition));
            upgradeButton.onClick.AddListener(() => UpgradeCivilianBuildingPopUp(buildingInPosition));
            buildButton.interactable = false; 
            upgradeButton.interactable = true;
            destroyButton.interactable = true;
        }
    }

    private void DestroyCivilianBuilding(Vector2 vector2)
    {
        GridSlot buildingGridSlot = popupOnGridSlot;
        PopupManager.Instance.ShowDestroyBuildingPopup(buildingGridSlot);
    }

    private void UpgradeCivilianBuildingPopUp(Vector2 vector2)
    {
        
    } 

    private void OpenCivilianBuildingsUIWithPopup(Vector2 vector2)
    {
       NavigationManager.Instance.OpenScreenCanvas(TabTypes.CivilianBuildings, true);
       CivilianBuildingsUIManager.Instance.BlockBuildingsWithLargerSizeInUIPanel(buildingSize);
    }

    private void OnCameraCenterCompleted()
    {
        civilianBuildUI.SetActive(true);
        CivilianBuildingUIBlocker.onPanelClick += OnPanelClick;
        HandleButtonsVisibility(popupOnGridSlot);

        _animator.SetTrigger("onEnable");
        civilianBuildUI.transform.position = Camera.main.WorldToScreenPoint(buildingInPosition);
    }

    public void CloseBuildUI()
    {
        if (civilianBuildUI.activeSelf)
        {
            _animator.SetTrigger("onDisable");
        }
            
    }

    public void DisableCivilianBuildUI()
    {
        CivilianBuildingUIBlocker.onPanelClick -= OnPanelClick;
        civilianBuildUI.SetActive(false);
        LevelGrid.Instance.DestroyGridBuildPrefab();
        // buildingSize = 0;
        CameraScroll.Instance.SetIfPlayerCanMoveCamera(true);
    }


}