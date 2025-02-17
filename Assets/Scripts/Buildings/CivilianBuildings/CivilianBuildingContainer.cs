using System;
using BuildingsTest;
using Game;
using GameResources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CivilianBuildingContainer : MonoBehaviour
{

    public CivilianBuildingsSO _civilianBuildingInfo { get; private set; }
    
    private Image _buildingIcon;
    private TextMeshProUGUI _buildingName;
    private TextMeshProUGUI _buildTimeText;
    private TextMeshProUGUI _upgradeTimeText;
    private TextMeshProUGUI _gridSizeText;
    private Image _resourceProductionIcon;
    private TextMeshProUGUI _resourceProductionText;

    private TextMeshProUGUI _buildingCost1Text;
    private Image _buildingCost1Image;
    private GameObject _resource2CostGameObject;
    private TextMeshProUGUI _buildingCost2Text;
    private Image _buildingCost2Image;
    private Button _containerButton;
    private GameObject _selectorObject;
    private GameObject _noSizeAdvisorGameObject;

    [SerializeField] private bool buttonIsActivated = false;
    private bool panelIsShowing;
    private void Awake()
    {
        SetReferences();
        _containerButton.interactable = false;
        buttonIsActivated = false;
    }

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
        if (_civilianBuildingInfo.buildingCost2.resourceType == ResourceType.Undefined)
        {
            //Check only resource 1
            HandleResourcesText(_civilianBuildingInfo.buildingCost1, _buildingCost1Text, true);
        }
        else
        {
            //Check both resources
            CheckBothResources();
        }
   
    }

    private void CheckBothResources()
    {
        if (HandleResourcesText(_civilianBuildingInfo.buildingCost1, _buildingCost1Text, false) && 
            HandleResourcesText(_civilianBuildingInfo.buildingCost2, _buildingCost2Text, false) )
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

    private void SelectBuilding()
    {
        ShowSelectorUI();
        CivilianBuildingsUIManager.Instance.SelectBuildingInMenu(this, _civilianBuildingInfo);
    }

    public void HideSelectorUI()
    {
        _selectorObject.SetActive(false);
    }

    public void ShowSelectorUI()
    {
        _selectorObject.SetActive(true);
    }

    public void BlockContainer()
    {
        _gridSizeText.color = Color.red;
        _noSizeAdvisorGameObject.SetActive(true);
    }  
    public void UnblockContainer()
    {
        _gridSizeText.color = Color.white;
        _noSizeAdvisorGameObject.SetActive(false);
    }

    public void StartRefreshingIfPlayerHasResources()
    {
        panelIsShowing = true;
    }
    public void StopRefreshingIfPlayerHasResources()
    {
        panelIsShowing = false;
    }
    /// <summary>
    /// Set properties of a single container
    /// </summary>
    /// <param name="civilianBuilding"></param>
    public void SetProperties(IBuildingsSO civilianBuilding)
    {
        _civilianBuildingInfo = civilianBuilding as CivilianBuildingsSO;
        _buildingIcon.sprite = _civilianBuildingInfo.buildingIcon;
        _buildingName.text = _civilianBuildingInfo.buildingName;

        _buildTimeText.text = _civilianBuildingInfo.timeToBuild.ToString();
        _upgradeTimeText.text = _civilianBuildingInfo.timeToUpgrade.ToString();
        _gridSizeText.text = _civilianBuildingInfo.buildSize.ToString();

        _resourceProductionText.text = _civilianBuildingInfo.resourceProduced.resourceProducedBaseLevel1.ToString();
        _resourceProductionIcon.sprite = CivilianBuildingsUIManager.Instance.GetSpriteFromResource(_civilianBuildingInfo.resourceProduced.resourceProduced);
        
        _buildingCost1Text.text = _civilianBuildingInfo.buildingCost1.cost.ToString();
        _buildingCost1Image.sprite = CivilianBuildingsUIManager.Instance.GetSpriteFromResource(_civilianBuildingInfo.buildingCost1.resourceType);
        if (civilianBuilding.buildingCost2.resourceType != ResourceType.Undefined)
        {
            _buildingCost2Text.text = _civilianBuildingInfo.buildingCost1.cost.ToString();
            _buildingCost2Image.sprite =
                CivilianBuildingsUIManager.Instance.GetSpriteFromResource(_civilianBuildingInfo.buildingCost2.resourceType);
        }
        else
        {
            _resource2CostGameObject.SetActive(false);
        }
    }
    private void SetReferences()
    {
        _buildingIcon = transform.Find("BuildingIconInfo/BuildingIcon").GetComponent<Image>();
        _buildingName = transform.Find("BuildingInfo/BuildingName").GetComponent<TextMeshProUGUI>();
        
        _buildTimeText = transform.Find("BuildingInfo/Timers/BuildTime/BuildTimeText").GetComponent<TextMeshProUGUI>();
        _upgradeTimeText = transform.Find("BuildingInfo/Timers/UpgradeTime/UpgradeTimeText").GetComponent<TextMeshProUGUI>();
        _gridSizeText = transform.Find("BuildingInfo/Timers/BuildSize/BuildSizeText").GetComponent<TextMeshProUGUI>();
        
        _resourceProductionIcon = transform.Find("BuildingInfo/Timers/ResourceProduction/ResourceProductionIcon").GetComponent<Image>();
        _resourceProductionText = transform.Find("BuildingInfo/Timers/ResourceProduction/ResourceProductionText").GetComponent<TextMeshProUGUI>();
        
        _buildingCost1Text = transform.Find("ResourcesCost/Button/ResourcesCost/Cost1/Resource1Cost").GetComponent<TextMeshProUGUI>();
        _buildingCost1Image = transform.Find("ResourcesCost/Button/ResourcesCost/Cost1/Resource1Icon").GetComponent<Image>();
        _resource2CostGameObject = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2").GameObject();
        _buildingCost2Text = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2/Resource2Cost").GetComponent<TextMeshProUGUI>();
        _buildingCost2Image = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2/Resource2Icon").GetComponent<Image>();

        _selectorObject = transform.Find("Selector").gameObject;
        _noSizeAdvisorGameObject = transform.Find("NoSizeForBuilding").gameObject;
        _containerButton = transform.Find("Button").GetComponent<Button>();
        _containerButton.onClick.AddListener(() => SelectBuilding());
    }

}
