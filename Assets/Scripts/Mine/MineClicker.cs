using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MineClicker : MonoBehaviour, IPointerClickHandler
{
    public static event EventHandler onMineClick;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Click in mine");
            //TODO: On each click, activate chop and hit animation in both the resource and character, all that are in
            onMineClick?.Invoke(this, EventArgs.Empty);
        }
    }
}