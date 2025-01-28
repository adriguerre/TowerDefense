using System;
using UnityEngine;

public class CivilianBuildingUIPanel : Singleton<CivilianBuildingUIPanel>
{
    
    private Canvas civilianBuilding;


    private void Awake()
    {
        civilianBuilding = GetComponentInParent<Canvas>();
    }
    
    public void OpenCivilianBuildingUI()
    {
        civilianBuilding.enabled = true;
        this.gameObject.SetActive(true);
    }
    
    public void CloseCivilianBuildingUI()
    {
        civilianBuilding.enabled = false;
        this.gameObject.SetActive(false);
    }
}
