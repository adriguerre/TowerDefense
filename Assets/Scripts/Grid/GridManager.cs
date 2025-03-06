using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager
{
    
    
    public int width { get; private set; }
    public int height { get; private set; }
    public GridSlot[,] grid { get; private set;}
    
    private float cellSize;
    private List<int> civilianBuildingsAlreadyMarked;
    
    public GridManager(int width, int height, float cellSize, LevelSO levelSO)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        civilianBuildingsAlreadyMarked = new List<int>();

        grid = new GridSlot[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                int buildingID = 0;
                int size = 0;
                GridPosition gridPosition = new GridPosition(x, y);
                GridPositionType gridPositionType = GetSlotType(levelSO, gridPosition, out buildingID, out size);
                grid[x, y] = CreateGridObject(this, gridPosition, gridPositionType, buildingID, size);
            }
        }
    }

    private GridPositionType GetSlotType(LevelSO levelMap, GridPosition gridPosition, out int buildingID, out int size)
    {
        buildingID = 0;
        size = 0;
        if (levelMap.pathList.Contains(gridPosition))
        {
            return GridPositionType.Path;
        }else if (levelMap.blockedSlotList.Contains(gridPosition))
        {
            return GridPositionType.Obstacle;
        }else if (levelMap.temporaryBlockedSlotsList.Contains(gridPosition))
        {
            return GridPositionType.TemporaryObstacle;
        }else if (IsACivilianBuildingPosition(levelMap, gridPosition,out buildingID, out size))
        {
            return GridPositionType.CivilianBuilding;
        }
        else
        {
            return GridPositionType.Free;
        }

        return GridPositionType.Free;
    }

    private bool IsACivilianBuildingPosition(LevelSO levelMap, GridPosition gridPosition, out int buildingID, out int size)
    {
        buildingID = 0;
        size = 0;
        //We will check if it is civilian building
        foreach (var CivilianBuilding in levelMap.CivilianBuildingGridPosisitions)
        {
            if (CivilianBuilding.gridPositionList.Contains(gridPosition))
            {
                buildingID = CivilianBuilding.buildingId;
                size = CivilianBuilding.size;
                return true;
            }
        }

        return false;
    }
    
    
    /// <summary>
    /// Replace this gridSlot to military building slot
    /// </summary>
    /// <param name="gridPosition"></param>
    public void AddMilitaryBuildingToGrid(GridPosition gridPosition)
    {
        grid[gridPosition.x, gridPosition.y]._gridPositionType = GridPositionType.MilitaryBuilding;
    }


    /// <summary>
    /// Creamos un grid slot especifico en la posicion designada
    /// </summary>
    /// <param name="gridManager"></param>
    /// <param name="gridPosition"></param>
    /// <param name="slotType"></param>
    /// Puede que buildingID sea 0, en ese caso no hay edificio en esa posicion
    /// <returns></returns>
    private GridSlot CreateGridObject(GridManager gridManager, GridPosition gridPosition, GridPositionType slotType, int buildingID, int buildingSize)
    {
        GridSlot newGridSlot = new GridSlot(gridPosition, null, slotType, buildingID, buildingSize);
        LevelGrid.Instance.InstantiateGridSlotPrefab(gridPosition);
        return newGridSlot;
    }
    
    public GridSlot GetGridSlotFromMousePosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(new Vector2(worldPosition.x, worldPosition.y));
        Debug.Log("Mouse Clicked in position: " + gridPosition);
        if (IsValidGridPosition(gridPosition))
        {
            return grid[gridPosition.x, gridPosition.y];
        }
        return null;

    }

    public GridSlot GetGridSlotFromGridPosition(GridPosition gridPosition)
    {
        return grid[gridPosition.x, gridPosition.y];
    }
    
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0
               && gridPosition.y >= 0
               && gridPosition.x < width
               && gridPosition.y < height;
    }

    public Vector2 GetTopRightCorner(GridPosition gridPosition)
    {
        float worldX = gridPosition.x * cellSize + (cellSize / 2);
        float worldY = gridPosition.y * cellSize + (cellSize / 2);

        return new Vector2(worldX, worldY);
    }
    public Vector2 GetTopPosition(GridPosition gridPosition)
    {
        float worldX = gridPosition.x * cellSize;
        float worldY = gridPosition.y * cellSize + (cellSize / 2);

        return new Vector2(worldX, worldY);
    }


}