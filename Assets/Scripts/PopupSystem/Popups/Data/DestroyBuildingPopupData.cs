using UnityEngine;

namespace PopupSystem.Popups.Data
{
    public class DestroyBuildingPopupData : APopupData
    {
        public GridSlot BuildingInGridSlot {get; private set;}
        
        public DestroyBuildingPopupData(Priority priority, string message, string title, GridSlot removingFromGridSlot) : base(priority, message, title)
        {
            BuildingInGridSlot = removingFromGridSlot;
        }
    }
}