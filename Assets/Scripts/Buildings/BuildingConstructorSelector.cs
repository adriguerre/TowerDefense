using System;
using Buildings.CivilianBuildings;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingConstructorSelector : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;
        
        [Header("Confirm Back Buttons")]
        [SerializeField] private GameObject ConfirmBackButtons;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        
        private CivilianBuildingsSO _civilianBuildingsSO;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            

        }

        private void CancelBuildingConstruction()
        {
            
            CivilianBuildingsUIManager.Instance.playerIsChoosingPlaceToCivilianBuild = false;
            LevelGrid.Instance.DestroyGridBuildPrefab();
            Destroy(this.gameObject);
        }

        public void ActivateBuildingConfirmOption(CivilianBuildingsSO civilianBuildingInfo)
        {
            _civilianBuildingsSO = civilianBuildingInfo;
            _spriteRenderer.color = Color.green;
            ConfirmBackButtons.SetActive(true);
            cancelButton.onClick.AddListener(() => CancelBuildingConstruction());
            confirmButton.onClick.AddListener(() => ConfirmBuildingConstruction());

        }

        public void ConfirmBuildingConstruction()
        {
            //TODO KW: Hay que modificar el positionTOBuild cada vez que movamos el selector a algun lado
            BuilderManager.Instance.BuildCivilianBuildings(_civilianBuildingsSO);
            Destroy(this.gameObject);
        }
    }
}