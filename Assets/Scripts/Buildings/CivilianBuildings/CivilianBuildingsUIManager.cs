using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CivilianBuildingsUIManager : ISingleton<CivilianBuildingsUIManager>
{
    [SerializeField] private GameObject civilianBuildingContainerPrefab;
    [SerializeField] private GameObject gridContainer;
    private List<CivilianBuildingsSO> civilianBuildings;


    [Header("Resources Sprites")] 
    [SerializeField] private Sprite foodSprite;
    [SerializeField] private Sprite woodSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite goldSprite;
    protected override void Awake()
    {
        base.Awake();
        civilianBuildings = new List<CivilianBuildingsSO>();
        civilianBuildings = Resources.LoadAll<CivilianBuildingsSO>("CivilianBuildings").ToList();
    }

    private void Start()
    {
        SpawnBuildingContainers();
    }

    private void SpawnBuildingContainers()
    {
        foreach (var building in civilianBuildings)
        {
            GameObject newBuilding = Instantiate(civilianBuildingContainerPrefab, Vector3.zero, Quaternion.identity,
                gridContainer.transform);
            
            newBuilding.GetComponent<CivilianBuildingContainer>().SetProperties(building);
        }
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