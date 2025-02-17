using System;
using BuildingsTest;
using NaughtyAttributes;
using GameResources;
using UnityEngine;

[CreateAssetMenu(fileName = "CivilianBuildingsSO", menuName = "ScriptableObjects/CivilianBuildings/CivilianBuildingsSO")]
public class CivilianBuildingsSO : IBuildingsSO
{
    public ResourceProduced resourceProduced;
}
