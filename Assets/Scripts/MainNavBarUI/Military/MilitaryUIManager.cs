using System;
using UnityEngine;

public class MilitaryUIManager : Singleton<MilitaryUIManager>
{
    
    private Canvas militaryCanvas;


    private void Awake()
    {
        militaryCanvas = GetComponentInParent<Canvas>();
    }
    
    public void OpenMilitaryUI()
    {
        militaryCanvas.enabled = true;
        this.gameObject.SetActive(true);
    }
    
    public void CloseMilitaryUI()
    {
        militaryCanvas.enabled = false;
        this.gameObject.SetActive(false);
    }
}
