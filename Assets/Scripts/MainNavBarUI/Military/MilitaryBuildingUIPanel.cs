using System;
using UnityEngine;

public class MilitaryBuildingUIPanel : Singleton<MilitaryBuildingUIPanel>
{
    
    public static Action onMilitaryBuildingOpened;
    public static Action onMilitaryBuildingClosed;
    
    private Canvas militaryCanvas;

    private void Awake()
    {
        militaryCanvas = GetComponentInParent<Canvas>();
    }
    
    public void OpenMilitaryUI()
    {
        militaryCanvas.enabled = true;
        this.gameObject.SetActive(true);
        CameraScroll.Instance.SetIfPlayerCanMoveCamera(false);
        onMilitaryBuildingOpened?.Invoke();
    }
    
    public void CloseMilitaryUI()
    {
        militaryCanvas.enabled = false;
        this.gameObject.SetActive(false);
        
        onMilitaryBuildingClosed?.Invoke();
    }
}
