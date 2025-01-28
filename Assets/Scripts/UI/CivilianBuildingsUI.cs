using System;
using UnityEngine;

public class CivilianBuildingsUI : ISingleton<CivilianBuildingsUI>
{
    private GameObject civilianBuildUI;
    [SerializeField] private Animator _animator;
    public Action onCameraCenterCompleted;
    private Vector2 buildingInPosition;
    protected override void Awake()
    {
        civilianBuildUI = this.gameObject.transform.Find("CivilianBuildingsBuildUI").gameObject;
        onCameraCenterCompleted += OnCameraCenterCompleted;
        _animator = GetComponent<Animator>();
    }




    private void OnPanelClick()
    { 
        CloseBuildUI();
    }

    public void OpenBuildUI(Vector2 position)
    {
        buildingInPosition = position;
        CameraScroll.Instance.CenterCameraOnBuilding(position.y, onCameraCenterCompleted);
        CameraScroll.Instance.canMoveCamera = false;

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
        CameraScroll.Instance.canMoveCamera = true;

    }


}