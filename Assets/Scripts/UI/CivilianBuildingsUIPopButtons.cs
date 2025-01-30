using System;
using MainNavBarUI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CivilianBuildingsUIPopButtons : ISingleton<CivilianBuildingsUIPopButtons>
{
    private GameObject civilianBuildUI;
    private Animator _animator;
    public Action onCameraCenterCompleted;
    private Vector2 buildingInPosition;
    private int buildingSize;

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
        Debug.Log("KW: SE HA PUESTO BUILDING SIZE EN " + gridSlot.buildingSize);
        buildingSize = gridSlot.buildingSize;
        buildingInPosition = position;
        CameraScroll.Instance.CenterCameraOnBuilding(position.y, onCameraCenterCompleted);
        CameraScroll.Instance.canMoveCamera = false;
        buildButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        destroyButton.onClick.RemoveAllListeners();
        if (gridSlot.GetBuildingInGridSlot() == null)
        {
            //Solo se puede construir, no upgradear ni destruir
            buildButton.onClick.AddListener(() => OpenCivilianBuildingsUI(buildingInPosition));
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
       
    }

    private void UpgradeCivilianBuildingPopUp(Vector2 vector2)
    {
        
    } 

    private void OpenCivilianBuildingsUI(Vector2 vector2)
    {
       NavigationManager.Instance.OpenScreenCanvas(TabTypes.CivilianBuildings);
       CivilianBuildingsUIManager.Instance.BlockBuildingsWithLargerSize(buildingSize);

    }

    private void OnCameraCenterCompleted()
    {
        civilianBuildUI.SetActive(true);
        CivilianBuildingUIBlocker.onPanelClick += OnPanelClick;
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
        //TODO KW: Hay que hacer un sistema para que ahora deje de clickar fuera y se pueda borrar, hemos creado el detectar si está en build ui
        CivilianBuildingUIBlocker.onPanelClick -= OnPanelClick;
        civilianBuildUI.SetActive(false);
       // buildingSize = 0;
        CameraScroll.Instance.canMoveCamera = true;

    }


}