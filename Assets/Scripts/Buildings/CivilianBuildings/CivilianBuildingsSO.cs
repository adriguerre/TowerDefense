using System;
using NaughtyAttributes;
using GameResources;
using UnityEngine;

[CreateAssetMenu(fileName = "CivilianBuildingsSO", menuName = "ScriptableObjects/CivilianBuildings/CivilianBuildingsSO")]
public class CivilianBuildingsSO : ScriptableObject
{
    private int[] sizeValues = new int[] { 4, 6 };
    
    public int buildingID;
    public Sprite buildingIcon;
    public string buildingName;
    [TextArea(10,30)]
    public string buildingDescription;
    public GameObject buildingPrefab;
    public int timeToBuild;
    public int timeToUpgrade;
    [Dropdown("sizeValues")]
    public int buildSize;
    
    public ResourceCost buildingCost1;
    //this can be null, indicating that only cost 1 resource
    public ResourceCost buildingCost2;
    public ResourceProduced resourceProduced;
}

[Serializable]
public class ResourceCost
{
    public ResourceType resourceType;
    public int cost;
}

[Serializable]
public class ResourceProduced
{
    public ResourceType resourceProduced;
    public int resourceProducedBaseLevel1;
    public int resourceProducedBaseLevel2;
    public int resourceProducedBaseLevel3;

    public int timeToProduceResourceBaseLevel1;
    public int timeToProduceResourceBaseLevel2;
    public int timeToProduceResourceBaseLevel3;
}
