using System;
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

    private TextMeshProUGUI _buildingCost1Text;
    private Image _buildingCost1Image;
    private GameObject _resource2CostGameObject;
    private TextMeshProUGUI _buildingCost2Text;
    private Image _buildingCost2Image;
    private Button containerButton;
    private GameObject selectorObject;

    public ResourceType resourceType1;

    private void Awake()
    {
        SetReferences();
    }

    private void SetReferences()
    {
        _buildingIcon = transform.Find("BuildingIconInfo/BuildingIcon").GetComponent<Image>();
        _buildingName = transform.Find("BuildingInfo/BuildingName").GetComponent<TextMeshProUGUI>();
        
        _buildTimeText = transform.Find("BuildingInfo/Timers/BuildTime/BuildTimeText").GetComponent<TextMeshProUGUI>();
        _upgradeTimeText = transform.Find("BuildingInfo/Timers/UpgradeTime/UpgradeTimeText").GetComponent<TextMeshProUGUI>();
        _gridSizeText = transform.Find("BuildingInfo/Timers/BuildSize/BuildSizeText").GetComponent<TextMeshProUGUI>();
        
        _buildingCost1Text = transform.Find("ResourcesCost/Button/ResourcesCost/Cost1/Resource1Cost").GetComponent<TextMeshProUGUI>();
        _buildingCost1Image = transform.Find("ResourcesCost/Button/ResourcesCost/Cost1/Resource1Icon").GetComponent<Image>();
        _resource2CostGameObject = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2").GameObject();
        _buildingCost2Text = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2/Resource2Cost").GetComponent<TextMeshProUGUI>();
        _buildingCost2Image = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2/Resource2Icon").GetComponent<Image>();

        selectorObject = transform.Find("Selector").gameObject;
        
        containerButton = transform.Find("Button").GetComponent<Button>();
        containerButton.onClick.AddListener(() => SelectBuilding());
    }

    private void SelectBuilding()
    {
        ShowSelectorUI();
        CivilianBuildingsUIManager.Instance.SelectBuildingInMenu(this, _civilianBuildingInfo);
    }

    public void HideSelectorUI()
    {
        selectorObject.SetActive(false);
    }

    public void ShowSelectorUI()
    {
        selectorObject.SetActive(true);
    }

    public void BlockContainer()
    {
        _gridSizeText.color = Color.red;
    }  
    public void UnblockContainer()
    {
        _gridSizeText.color = Color.white;
    }

    public void SetProperties(CivilianBuildingsSO civilianBuilding)
    {
        _civilianBuildingInfo = civilianBuilding;
        _buildingIcon.sprite = _civilianBuildingInfo.buildingIcon;
        _buildingName.text = _civilianBuildingInfo.buildingName;

        _buildTimeText.text = _civilianBuildingInfo.timeToBuild.ToString();
        _upgradeTimeText.text = _civilianBuildingInfo.timeToUpgrade.ToString();
        _gridSizeText.text = _civilianBuildingInfo.buildSize.ToString();
        
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
}
