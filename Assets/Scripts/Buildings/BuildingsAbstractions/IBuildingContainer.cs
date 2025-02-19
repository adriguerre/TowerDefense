using Game;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingsTest
{
    public abstract class IBuildingContainer : MonoBehaviour
    {
        public IBuildingsSO _buildingInfo { get; private set; }
        
        protected Image _buildingIcon;
        protected TextMeshProUGUI _buildingName;
        protected TextMeshProUGUI _buildTimeText;
        protected TextMeshProUGUI _upgradeTimeText;
        protected TextMeshProUGUI _gridSizeText;
        
        protected TextMeshProUGUI _buildingCost1Text;
        protected Image _buildingCost1Image;
        
        protected GameObject _resource2CostGameObject;
        protected TextMeshProUGUI _buildingCost2Text;
        protected Image _buildingCost2Image;
        
        protected Button _containerButton;
        protected GameObject _selectorObject;
        
        [SerializeField] protected bool buttonIsActivated = false;
        
        protected bool panelIsShowing;

        private void Update()
        {
            if (!panelIsShowing)
            {
                return;
            }

            if (TestingManager.Instance != null && TestingManager.Instance.ResourcesNotNeeded)
            {
                _containerButton.interactable = true;
                buttonIsActivated = true;
                return;
            }
            if (_buildingInfo.buildingCost2.resourceType == ResourceType.Undefined)
            {
                //Check only resource 1
                HandleResourcesText(_buildingInfo.buildingCost1, _buildingCost1Text, true);
            }
            else
            {
                //Check both resources
                CheckBothResources();
            }
        }
        private void CheckBothResources()
        {
            if (HandleResourcesText(_buildingInfo.buildingCost1, _buildingCost1Text, false) && 
                HandleResourcesText(_buildingInfo.buildingCost2, _buildingCost2Text, false) )
            {
                if (!buttonIsActivated)
                {
                    _containerButton.interactable = true;
                    buttonIsActivated = true;
                }
            }
            else
            {
                if (buttonIsActivated)
                {
                    _containerButton.interactable = false;
                    buttonIsActivated = false;
                }
            }
        }
        
        private bool HandleResourcesText(ResourceCost cost, TextMeshProUGUI costText, bool blockButton)
        {
            //Dont have resources
            if (!ResourcesManager.Instance.GetIfHasResources(cost))
            {
                costText.color = Color.red;
                if (blockButton)
                {
                    if (buttonIsActivated)
                    {
                        _containerButton.interactable = false;
                        buttonIsActivated = false;
                    }
                }
                return false;
            }
            //Has resources
            costText.color = Color.white;
            if (blockButton)
            {
                if (!buttonIsActivated)
                {
                    _containerButton.interactable = true;
                    buttonIsActivated = true;
                }
            }
            return true;
        }
        
        public void HideSelectorUI()
        {
            _selectorObject.SetActive(false);
        }

        public void ShowSelectorUI()
        {
            _selectorObject.SetActive(true);
        }
        
        public void StartRefreshingIfPlayerHasResources()
        {
            panelIsShowing = true;
        }
        public void StopRefreshingIfPlayerHasResources()
        {
            panelIsShowing = false;
        }

        public virtual void SetProperties(IBuildingsSO buildingInfoSO)
        {
            this._buildingInfo = buildingInfoSO; 
            _buildingIcon.sprite = _buildingInfo.buildingIcon;
            _buildingName.text = _buildingInfo.buildingName;

            _buildTimeText.text = _buildingInfo.timeToBuild.ToString() + "s";
            _upgradeTimeText.text = _buildingInfo.timeToUpgrade.ToString() + "s";
            _gridSizeText.text = _buildingInfo.buildSize.ToString();

        
            _buildingCost1Text.text = _buildingInfo.buildingCost1.cost.ToString();
            _buildingCost1Image.sprite = ResourcesManager.Instance.
                GetSpriteFromResource(_buildingInfo.buildingCost1.resourceType);
            if (buildingInfoSO.buildingCost2.resourceType != ResourceType.Undefined)
            {
                _buildingCost2Text.text = _buildingInfo.buildingCost1.cost.ToString();
                _buildingCost2Image.sprite =
                    ResourcesManager.Instance.GetSpriteFromResource(_buildingInfo.buildingCost2.resourceType);
            }
            else
            {
                _resource2CostGameObject.SetActive(false);
            }
        }

        protected abstract void SetReferences();

    }
}