using PopupSystem.PopupFactory;
using PopupSystem.Popups.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PopupSystem
{
    public class PopupManager : ISingleton<PopupManager>
    {
        [SerializeField] private PopupsLibrary _popupsLibrary;
        [SerializeField] private Transform _prefabsRoot;
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private Button _buttonMessage;
        [SerializeField] private Button _buttonUrgent;
        private IPopupMessageService _popupMessageService;

        private void Start()
        {
            _popupMessageService = new PopupMessageService(new PopupFactory.PopupFactory(_popupsLibrary), _prefabsRoot);
           /* _buttonSettings.onClick.AddListener(() =>
                _popupMessageService.PushPopup(new SettingsPopupData(Priority.Low, "Some settings text", "Settings")));
            _buttonMessage.onClick.AddListener(() =>
                _popupMessageService.PushPopup(new MessagePopupData(Priority.High, "Some message text", "Message")));
            _buttonUrgent.onClick.AddListener(() =>
                _popupMessageService.PushPopup(new ErrorPopupData(Priority.Urgent, "Urgent error", "Error")));*/
        }
        public void ShowWorkInProgressPopup()
        {
            _popupMessageService.PushPopup(new InfoPopupData(Priority.Low, "This feature is still in development. Stay tuned for updates!", "Work In Progress"));
        }

        public void ShowInfoPopup(Priority priority, string message, string title)
        {
            _popupMessageService.PushPopup(new InfoPopupData(priority, message, title));
        }
        
    }
}