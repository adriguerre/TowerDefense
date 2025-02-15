using UnityEngine;

namespace PopupSystem.PopupFactory
{
    public interface IPopupFactory
    {
        /// <summary>
        /// Create a new instance of APopup based on input data
        /// </summary>
        /// <param name="popupData">Popup data</param>
        /// <param name="parent">Popup parent tranform</param>
        /// <returns>Returns new APopup</returns>
        APopup InstantiatePopup(APopupData popupData, Transform parent);
    }
}