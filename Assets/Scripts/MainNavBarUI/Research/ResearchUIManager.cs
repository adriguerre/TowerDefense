using System;
using UnityEngine;

public class ResearchUIManager : Singleton<ResearchUIManager>
{
    
    private Canvas researchCanvas;


    private void Awake()
    {
        researchCanvas = GetComponentInParent<Canvas>();
    }
    
    public void openResearchUI()
    {
        researchCanvas.enabled = true;
        this.gameObject.SetActive(true);
    }
    
    public void CloseResearchUI()
    {
        researchCanvas.enabled = false;
        this.gameObject.SetActive(false);
    }
}
