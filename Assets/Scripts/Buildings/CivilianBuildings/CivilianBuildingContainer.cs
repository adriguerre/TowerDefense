using System;
using BuildingsTest;
using Game;
using GameResources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CivilianBuildingContainer : IBuildingContainer
{
    public CivilianBuildingsSO _civilianBuildingInfo { get; private set; }
    
    private Image _resourceProductionIcon;
    private TextMeshProUGUI _resourceProductionText;
    
    private GameObject _noSizeAdvisorGameObject;

    private void Awake()
    {
        SetReferences();
        _containerButton.interactable = false;
        buttonIsActivated = false;
    }
    
    private void SelectBuilding()
    {
        ShowSelectorUI();
        CivilianBuildingsUIManager.Instance.SelectBuildingInMenu(this, _civilianBuildingInfo);
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
    
    /// <summary>
    /// Set properties of a single container
    /// </summary>
    /// <param name="civilianBuilding"></param>
    public override void SetProperties(IBuildingsSO civilianBuilding)
    {
        base.SetProperties(civilianBuilding);
        _civilianBuildingInfo = civilianBuilding as CivilianBuildingsSO;
        _resourceProductionText.text = _civilianBuildingInfo.resourceProduced.resourceProducedBaseLevel1.ToString();
        _resourceProductionIcon.sprite = CivilianBuildingsUIManager.Instance.GetSpriteFromResource(_civilianBuildingInfo.resourceProduced.resourceProduced);
        
    }
    protected override void SetReferences()
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
