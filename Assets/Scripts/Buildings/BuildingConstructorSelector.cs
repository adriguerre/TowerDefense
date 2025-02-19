using System;
using Buildings.CivilianBuildings;
using Buildings.MilitaryBuildings;
using BuildingsTest;
using CDebugger;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingConstructorSelector : MonoBehaviour
    {

        private GameObject constructionPlaceholder;
        private SpriteRenderer _spriteRenderer;
        
        [Header("Confirm Back Buttons")]
        [SerializeField] private GameObject ConfirmBackButtons;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        private bool isTryingToBuildCivilianBuilding = false;
        
        public IBuildingsSO CivilianBuildingsSO { get; private set; }
        
        private void Awake()
        {
            constructionPlaceholder = transform.Find("Construction").gameObject;
            _spriteRenderer = constructionPlaceholder.transform.Find("BuildingSprite").GetComponent<SpriteRenderer>();
        }


        public void ActivateBuildingConfirmOption(IBuildingsSO civilianBuildingInfo, bool isCivilianBuilding)
        {
            CivilianBuildingsSO = civilianBuildingInfo;
            constructionPlaceholder.SetActive(true);
            isTryingToBuildCivilianBuilding = isCivilianBuilding;
            _spriteRenderer.color = Color.green;
            ConfirmBackButtons.SetActive(true);
            cancelButton.onClick.AddListener(() => CancelBuildingConstruction());
            confirmButton.onClick.AddListener(() => ConfirmBuildingConstruction());

        }
        
        private void CancelBuildingConstruction()
        {
            if (isTryingToBuildCivilianBuilding) //cancel civilian building
            {
                ConstructionBuildBlocker.Instance.DestroyCivilianBuildingsSpawnBlockers();
                CivilianBuildingsUIManager.Instance.playerIsTryingToStartConstruction = false;
            }
            else //Cancel military building
            {
                ConstructionBuildBlocker.Instance.DestroyMilitaryBuildingsSpawnBlockers();
                MilitaryBuildingsUIManager.Instance.playerIsTryingToStartConstruction = false;
            }

            LevelGrid.Instance.DestroyGridBuildPrefab();
            Destroy(this.gameObject);
        }

        public void ConfirmBuildingConstruction()
        {
            if (isTryingToBuildCivilianBuilding)
            {
                ConstructionBuildBlocker.Instance.DestroyCivilianBuildingsSpawnBlockers();
                BuilderManager.Instance.BuildCivilianBuildings(CivilianBuildingsSO);
            }
            else
            {
                ConstructionBuildBlocker.Instance.DestroyMilitaryBuildingsSpawnBlockers();
                CustomDebugger.Log(LogCategories.MilitaryBuildings, "Add building to builder manager");
                //BuilderManager.Instance.BuildCivilianBuildings(CivilianBuildingsSO);
            }

            Destroy(this.gameObject);

        }
    }
}