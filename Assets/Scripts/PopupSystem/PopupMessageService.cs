using System.Collections.Generic;
using System.Linq;
using PopupSystem.PopupFactory;
using UnityEngine;

namespace PopupSystem
{
    public class PopupMessageService : IPopupMessageService
    {
        
        private List<APopupData> _popupsToShow;
        private APopup _openedPopup;
        private APopup _hidedPopup;
        private readonly IPopupFactory _popupsFactory;
        private readonly Transform _prefabsRoot;
        
        public PopupMessageService(IPopupFactory popupsFactory, Transform prefabsRoot)
        {
            _popupsFactory = popupsFactory;
            _prefabsRoot = prefabsRoot;
            _popupsToShow = new List<APopupData>();
        }
        
        public void PushPopup(APopupData data)
        {
            if (data == null)
            {
                Debug.LogError($"[{nameof(PopupMessageService)}] - Unable to push message. Null APopupData");
                return;
            }
            
            _popupsToShow ??= new List<APopupData>();
            _popupsToShow.Add(data);
            ShowPopup();
        }

        private APopupData GetPopupToShow()
        {
            if (getQueueSize() == 0)
                return null;

            var prioritizedList = _popupsToShow.OrderByDescending(t => t.Priority);
            return prioritizedList.FirstOrDefault();
        }

        private void ShowPopup()
        {
            //check popups, if we have hidden popup, reopen it
            if (_openedPopup == null && _hidedPopup != null)
            {
                _hidedPopup.Open();
                _openedPopup = _hidedPopup;
                _hidedPopup = null;
                return;
            }
            
            var popupData = GetPopupToShow();
            //check if system need to show popup, or we have already opened popup
            if (popupData == null || (_openedPopup != null && popupData.Priority != Priority.Urgent))
                return;
            
            //check if the system needs to close already opened popup and show Urgent popup
            if (popupData.Priority == Priority.Urgent && _openedPopup != null)
            {
                _openedPopup.Hide();
                _hidedPopup = _openedPopup;
                _openedPopup = null;
            }
            
            //remove popup from the queue and instantiate it with the IPopupFactory
            _popupsToShow.Remove(popupData);
            _openedPopup = _popupsFactory.InstantiatePopup(popupData, _prefabsRoot);
            _openedPopup.OnClosed += PopupOnClosedHandler;
            _openedPopup.Open();
        }

        
        private void PopupOnClosedHandler(PopupResultData obj)
        {
            _openedPopup.OnClosed -= PopupOnClosedHandler;
            _openedPopup = null;
            ShowPopup();
        }
        public int getQueueSize()
        {
            return _popupsToShow?.Count ?? 0;
        }

        public APopup GetOpenedPopup()
        {
            return _openedPopup;
        }
        
    }
}