using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO", order = 1)]
[Serializable]
public class LevelSO : ScriptableObject
{

    public string levelName;
    public Sprite levelVillageIcon;
    public Sprite levelGridVisualizer;
    [TextArea(10,100)]
    public string levelDescription;
    public List<GridPosition> pathList;
    public List<GridPosition> blockedSlotList;
    public List<GridPosition> temporaryBlockedSlotsList;
    public List<CivilianBuildingGridPosisition> CivilianBuildingGridPosisitions;
}

[Serializable]
public class CivilianBuildingGridPosisition
{
    public int buildingId;
    public int size;
    /// <summary>
    /// La primera posición de esta lista, deberá de ser siempre la esquina de abajo izquierda
    /// </summary>
    public List<GridPosition> gridPositionList;
}
