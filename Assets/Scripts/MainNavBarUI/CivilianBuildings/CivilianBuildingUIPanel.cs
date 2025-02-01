using System;
using UnityEngine;

public class CivilianBuildingUIPanel : Singleton<CivilianBuildingUIPanel>
{
    
    private Canvas civilianBuilding;

    public static Action onCivilianBuildingOpenedWithoutPopup;

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
        
    }
    
    public void CloseCivilianBuildingUI()
    {
        civilianBuilding.enabled = false;
        this.gameObject.SetActive(false);
    }
}
