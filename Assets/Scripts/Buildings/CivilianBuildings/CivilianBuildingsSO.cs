using System;
using BuildingsTest;
using NaughtyAttributes;
using GameResources;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "ScriptableObjects/CivilianBuildings/BuildingSO")]
public class CivilianBuildingsSO : IBuildingsSO
{
    public ResourceProduced resourceProduced;
}
