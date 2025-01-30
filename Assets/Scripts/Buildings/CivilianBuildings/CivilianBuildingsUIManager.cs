using System;
using System.Collections.Generic;
using System.Linq;
using GameResources;
using MainNavBarUI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// This script is responsible of all the UI from the panel CivilianBuildings
/// </summary>
public class CivilianBuildingsUIManager : ISingleton<CivilianBuildingsUIManager>
{
    [SerializeField] private GameObject civilianBuildingContainerPrefab;
    [SerializeField] private Button buildButton;
    private TextMeshProUGUI buildingButtonText;
    [SerializeField] private GameObject gridContainerInCanvas;
    private List<CivilianBuildingContainer> civilianBuildingContainersList;
    private List<CivilianBuildingsSO> _civilianBuildings;
    private CivilianBuildingsSO _currentSelectedCivilianBuilding;
    private CivilianBuildingContainer _currentContainerSelected;

    public Action<CivilianBuildingsSO> OnBuildingStarted;
    
    [Header("Resources Sprites")] 
    [SerializeField] private Sprite foodSprite;
    [SerializeField] private Sprite woodSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite goldSprite;
    protected override void Awake()
    {
        base.Awake();
        _civilianBuildings = new List<CivilianBuildingsSO>();
        _civilianBuildings = Resources.LoadAll<CivilianBuildingsSO>("CivilianBuildings").ToList();
        civilianBuildingContainersList = new List<CivilianBuildingContainer>();
        buildingButtonText = buildButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        CivilianBuildingUIPanel.onCivilianBuildingOpened += onUIOpened;
        SpawnBuildingContainers();
        buildButton.onClick.AddListener(() => BuildCivilianBuilding());
    }

    private void onUIOpened()
    {
        UnblockAllBuildings();
    }

    private void BuildCivilianBuilding()
    {
        if (_currentContainerSelected == null || _currentSelectedCivilianBuilding == null)
            return;
        
        //TODO KW: Check resources
        Debug.Log("KW: BUYING CIVILIAN BUILDING");
        //TODO KW: Cambiar el modo de pasar argumentos a ese método para no tener que buscar todo el rato ( si es madera pasa x, si es lo otro pasa y)
        //if(ResourcesManager.Instance.TryToSpendResources())
        
        //TODO KW: Comprobar que no hay nada construido encima
        //TODO KW: Comprobar que cabe dentro de los huecos, es decir si es hueco 4, que no se pueda construir hueco 6
        OnBuildingStarted?.Invoke(_currentSelectedCivilianBuilding);
        LevelGrid.Instance.currentGridSlot.AddCivilianBuildingToAllSlot(_currentSelectedCivilianBuilding);
        NavigationManager.Instance.CloseCurrentTab();
        CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
    }

    private void SpawnBuildingContainers()
    {
        foreach (var building in _civilianBuildings)
        {
            GameObject newBuilding = Instantiate(civilianBuildingContainerPrefab, Vector3.zero, Quaternion.identity,
                gridContainerInCanvas.transform);
            CivilianBuildingContainer container = newBuilding.GetComponent<CivilianBuildingContainer>();
            container.SetProperties(building);
            civilianBuildingContainersList.Add(container);
        }
    }

    public void BlockBuildingsWithLargerSize(int size)
    {
        foreach (var containers in civilianBuildingContainersList)
        {
            Debug.Log("KW COMPARING: " + containers._civilianBuildingInfo.buildSize + " | " +  size);
            if (containers._civilianBuildingInfo.buildSize > size)
            {
                containers.BlockContainer();
            }
        }
    }
    
    public void UnblockAllBuildings()
    {
        foreach (var containers in civilianBuildingContainersList)
        {
            containers.UnblockContainer();
        }
    }

    public void SelectBuildingInMenu(CivilianBuildingContainer ContainerSelected, CivilianBuildingsSO civilianBuilding)
    {
        if (civilianBuilding == _currentSelectedCivilianBuilding)
            return;
        
        _currentSelectedCivilianBuilding = civilianBuilding;
        if (_currentContainerSelected != null)
        {
            _currentContainerSelected.HideSelectorUI();
        }
        _currentContainerSelected = ContainerSelected;
        buildingButtonText.text = "Build " + _currentSelectedCivilianBuilding.buildingName;

    }

    public void UnSelectBuildingInMenu()
    {
        _currentSelectedCivilianBuilding = null;
        if (_currentContainerSelected != null)
        {
            _currentContainerSelected.HideSelectorUI();
        }
        _currentContainerSelected = null;
        buildingButtonText.text = "Build";
    }

    public Sprite GetSpriteFromResource(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Undefined:
                break; 
            case ResourceType.Food:
                return foodSprite;
                break; 
            case ResourceType.Wood:
                return woodSprite;
                break; 
            case ResourceType.Stone:
                return stoneSprite;
                break; 
            case ResourceType.Iron:
                return ironSprite;
                break; 
            case ResourceType.Gold:
                return goldSprite;
                break; 
        }

        return null;
    }
}