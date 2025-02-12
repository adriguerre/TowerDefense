using System;
using UnityEngine;

public class CivilianBuildingUIPanel : Singleton<CivilianBuildingUIPanel>
{
    
    private Canvas civilianBuilding;

    public static Action onCivilianBuildingOpenedWithoutPopup;
    public static Action onCivilianBuildingOpened;
    public static Action onCivilianBuildingClosed;

    private void Awake()
    {
        civilianBuilding = GetComponentInParent<Canvas>();
    }
    
    public void OpenCivilianBuildingUI(bool isComingFromPopUp)
    {
        civilianBuilding.enabled = true;
        CameraScroll.Instance.SetIfPlayerCanMoveCamera(false);
        this.gameObject.SetActive(true);
        if(!isComingFromPopUp)
            onCivilianBuildingOpenedWithoutPopup?.Invoke();
        
        onCivilianBuildingOpened?.Invoke();
        
    }
    
    public void CloseCivilianBuildingUI()
    {
        civilianBuilding.enabled = false;
        this.gameObject.SetActive(false);
        onCivilianBuildingClosed?.Invoke();

    }
}
