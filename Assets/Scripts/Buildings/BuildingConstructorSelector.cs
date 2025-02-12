using System;
using Buildings.CivilianBuildings;
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
        
        public CivilianBuildingsSO CivilianBuildingsSO { get; private set; }
        
        private void Awake()
        {
            constructionPlaceholder = transform.Find("Construction").gameObject;
            _spriteRenderer = constructionPlaceholder.transform.Find("BuildingSprite").GetComponent<SpriteRenderer>();
        }


        public void ActivateBuildingConfirmOption(CivilianBuildingsSO civilianBuildingInfo)
        {
            CivilianBuildingsSO = civilianBuildingInfo;
            constructionPlaceholder.SetActive(true);
            _spriteRenderer.color = Color.green;
            ConfirmBackButtons.SetActive(true);
            cancelButton.onClick.AddListener(() => CancelBuildingConstruction());
            confirmButton.onClick.AddListener(() => ConfirmBuildingConstruction());

        }
        
        private void CancelBuildingConstruction()
        {
            CivilianConstructionBuildBlocker.Instance.DestroySpawnBlockers();
            CivilianBuildingsUIManager.Instance.playerIsChoosingPlaceToCivilianBuild = false;
            LevelGrid.Instance.DestroyGridBuildPrefab();
            Destroy(this.gameObject);
        }

        public void ConfirmBuildingConstruction()
        {
            CivilianConstructionBuildBlocker.Instance.DestroySpawnBlockers();
            BuilderManager.Instance.BuildCivilianBuildings(CivilianBuildingsSO);
            Destroy(this.gameObject);
        }
    }
}