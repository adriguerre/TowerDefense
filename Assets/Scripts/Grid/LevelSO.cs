using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO", order = 1)]
[Serializable]
public class LevelSO : ScriptableObject
{
    public List<GridPosition> pathList;
    public List<GridPosition> blockedSlotList;
    public List<GridPosition> temporaryBlockedSlotsList;
    public List<CivilianBuildingGridPosisition> CivilianBuildingGridPosisitions;
}

[Serializable]
public class CivilianBuildingGridPosisition
{
    public List<GridPosition> gridPositionList;
}
