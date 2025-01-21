using System;
using UnityEngine;

public class GridSlotHolder : MonoBehaviour
{
    private GridSlot currentGridSlot;
    
    private bool isHoldingMouse = false; // If Button is being pressed
    private float holdTimer = 0.0f;      // Current Timer
    private float holdThreshold = 1f; // Timer


    void Update()
    {
        // Detect if is beign pressed
        if (Input.GetMouseButtonDown(0))
        {
            isHoldingMouse = true; // Start
            holdTimer = 0.0f;      // Reset Timer
        }
        if (isHoldingMouse && Input.GetMouseButton(0))
        {
            holdTimer += Time.deltaTime; // Increase Timer
            if (holdTimer >= holdThreshold)
            {
                CompleteHoldAction();   // Completed -> Show UI
                isHoldingMouse = false; // Reset
            }
        }

        // Detect if click is released
        if (Input.GetMouseButtonUp(0))
        {

            isHoldingMouse = false;
        }
    }

    private void CompleteHoldAction()
    {
        Debug.Log("Hold action completed!");
        //Show UI
    }

    public void SetProperties(GridSlot newGridSlot)
    {
        currentGridSlot = newGridSlot;
    }

}