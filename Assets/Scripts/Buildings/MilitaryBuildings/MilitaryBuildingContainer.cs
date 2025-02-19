using BuildingsTest;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings.MilitaryBuildings
{
    public class MilitaryBuildingContainer : IBuildingContainer
    {
        public MilitaryBuildingsSO _militaryBuildingInfo { get; private set; }

        [SerializeField] private TextMeshProUGUI enemiesTarget;
        [SerializeField] private TextMeshProUGUI canPlaceInRoad;

        private void Awake()
        {
            SetReferences();
            _containerButton.interactable = false;
            buttonIsActivated = false;
        }
        private void SelectBuilding()
        {
            ShowSelectorUI();
            MilitaryBuildingsUIManager.Instance.SelectBuildingInMenu(this, _militaryBuildingInfo);
        }
        
        /// <summary>
        /// Set properties of a single container
        /// </summary>
        /// <param name="civilianBuilding"></param>
        public override void SetProperties(IBuildingsSO civilianBuilding)
        {
            base.SetProperties(civilianBuilding);
            _militaryBuildingInfo = civilianBuilding as MilitaryBuildingsSO;
            
            enemiesTarget.text = _militaryBuildingInfo.singleTarget ? "Single Target" : "Multiple Targets";
            canPlaceInRoad.text = _militaryBuildingInfo.canPlaceInRoad ? "Can Place In Road" : "Can't Place In Road";
        }
        
        //TODO: CHANGE THIS TO NEWEST CONTAINER REFERENCES
        protected override void SetReferences()
        {
            _buildingIcon = transform.Find("BuildingIconInfo/BuildingIcon").GetComponent<Image>();
            _buildingName = transform.Find("BuildingInfo/BuildingName").GetComponent<TextMeshProUGUI>();
        
            _buildTimeText = transform.Find("BuildingInfo/Timers/BuildTime/BuildTimeText").GetComponent<TextMeshProUGUI>();
            _upgradeTimeText = transform.Find("BuildingInfo/Timers/UpgradeTime/UpgradeTimeText").GetComponent<TextMeshProUGUI>();
            _gridSizeText = transform.Find("BuildingInfo/Timers/BuildSize/BuildSizeText").GetComponent<TextMeshProUGUI>();
        
            enemiesTarget = transform.Find("BuildingInfo/EnemiesTarget/EnemiesTargetText").GetComponent<TextMeshProUGUI>();
            canPlaceInRoad = transform.Find("BuildingInfo/CanPlaceInRoad/CanPlaceInRoadText").GetComponent<TextMeshProUGUI>();
        
            
            _buildingCost1Text = transform.Find("ResourcesCost/Button/ResourcesCost/Cost1/Resource1Cost").GetComponent<TextMeshProUGUI>();
            _buildingCost1Image = transform.Find("ResourcesCost/Button/ResourcesCost/Cost1/Resource1Icon").GetComponent<Image>();
            _resource2CostGameObject = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2").GameObject();
            _buildingCost2Text = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2/Resource2Cost").GetComponent<TextMeshProUGUI>();
            _buildingCost2Image = transform.Find("ResourcesCost/Button/ResourcesCost/Cost2/Resource2Icon").GetComponent<Image>();

            _selectorObject = transform.Find("Selector").gameObject;
            _containerButton = transform.Find("Button").GetComponent<Button>();
            _containerButton.onClick.AddListener(() => SelectBuilding());
        }
    }
}