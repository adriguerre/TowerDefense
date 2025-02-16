using System.Linq;
using UnityEngine;

namespace PopupSystem.PopupFactory
{
    public class PopupFactory : IPopupFactory
    {
        private readonly PopupsLibrary _popupsLibrary;
        
        public PopupFactory(PopupsLibrary popupsLibrary)
        {
            _popupsLibrary = popupsLibrary;
        }

        public APopup InstantiatePopup(APopupData popupData, Transform parent)
        {
            //check input data for null
            if (popupData == null || parent == null)
            {
                Debug.LogError($"[{nameof(PopupFactory)}] - Unable to instantiate popup. Null data");
                return null;
            }
            
            //get popup prefab from the popups library
            //system search for the prefab by the input APopupData type
            Debug.Log(popupData.GetType().Name);
            var data = _popupsLibrary.PopupItems.FirstOrDefault(t => t.Type.Equals(popupData.GetType().Name));
            if (data == null)
            {
                Debug.LogError($"[{nameof(PopupFactory)}] - Unable to instantiate popup. Can't find {nameof(APopupData)} popup");
                return null;
            }
            
            //here we check if the prefab is set in the data
            if (data.Popup == null)
            {
                Debug.LogError($"[{nameof(PopupFactory)}] - Unable to instantiate popup. {nameof(APopupData)} popup is null");
                return null;
            }
            
            //instantiate popup and return it
            var popup = GameObject.Instantiate(data.Popup, parent);
            popup.Setup(popupData);
            return popup;
        }
    }
}