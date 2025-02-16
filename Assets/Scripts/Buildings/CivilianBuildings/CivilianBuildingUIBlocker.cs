using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CivilianBuildingUIBlocker : MonoBehaviour, IPointerClickHandler
{

    public static Action onPanelClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onPanelClick?.Invoke();
    }
}
