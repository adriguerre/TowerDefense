using Buildings.CivilianBuildings;
using PopupSystem.Popups.Data;
using UnityEngine;

namespace PopupSystem.Popups.View
{
    public class DestroyBuildingPopupView : APopup
    {
        protected override void Awake()
        {
            base.Awake();
            
            _acceptButton.onClick.AddListener(() => RemoveBuildingFromGridSlot());
        }

        private void RemoveBuildingFromGridSlot()
        {
            DestroyBuildingPopupData destroyPopupData = _popupData as DestroyBuildingPopupData;
            Debug.Log("REMOVE BUILDING FROM " + destroyPopupData.BuildingInGridSlot.GetBuildingInGridSlot().buildingName);
            //We use the buildingID from the location place (map id)
            CivilianBuildingsManager.Instance.DestroyBuilding(destroyPopupData.BuildingInGridSlot.buildingID);
            //Reset map properties in this location (no building in all grid slots)
            LevelGrid.Instance.UnlinkBuildingFromAllCloseSlots(destroyPopupData.BuildingInGridSlot);
            CivilianBuildingsUIPopButtons.Instance.DisableCivilianBuildUI();

            Close();
        }
    }
}