using System;
using UnityEngine;

public class AbilitiesUIManager : Singleton<AbilitiesUIManager>
{
    
    private Canvas abilitiesCanvas;


    private void Awake()
    {
        abilitiesCanvas = GetComponentInParent<Canvas>();
    }
    
    public void OpenAbilitiesUI()
    {
        abilitiesCanvas.enabled = true;
        this.gameObject.SetActive(true);
    }
    
    public void CloseAbilitiesUI()
    {
        abilitiesCanvas.enabled = false;
        this.gameObject.SetActive(false);
    }
}
