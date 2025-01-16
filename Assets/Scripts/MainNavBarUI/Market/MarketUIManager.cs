using System;
using UnityEngine;

public class MarketUIManager : Singleton<MarketUIManager>
{
    
    private Canvas marketCanvas;


    private void Awake()
    {
        marketCanvas = GetComponentInParent<Canvas>();
    }
    
    public void OpenMarketUI()
    {
        marketCanvas.enabled = true;
        this.gameObject.SetActive(true);
    }
    
    public void CloseMarketUI()
    {
        marketCanvas.enabled = false;
        this.gameObject.SetActive(false);
    }
}
