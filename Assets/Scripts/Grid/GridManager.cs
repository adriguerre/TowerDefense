using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager
{
    
    #region Private Fields
    
    public int width { get; private set; }
    public int height { get; private set; }
    private float cellSize;

    public GridSlot[,] grid { get; private set;}
    
    
    #endregion 	


    public GridManager(int width, int height, float cellSize, LevelSO levelSO)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        grid = new GridSlot[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                GridPositionType gridPositionType = GetSlotType(levelSO, gridPosition);
                grid[x, y] = CreateGridObject(this, gridPosition, gridPositionType);
            }
        }
    }

    private GridPositionType GetSlotType(LevelSO levelMap, GridPosition gridPosition)
    {
        if (levelMap.pathList.Contains(gridPosition))
        {
            return GridPositionType.Path;
        }else if (levelMap.blockedSlotList.Contains(gridPosition))
        {
            return GridPositionType.Obstacle;
        }else if (levelMap.temporaryBlockedSlotsList.Contains(gridPosition))
        {
            return GridPositionType.TemporaryObstacle;
        }
        else
        {
            return GridPositionType.Free;
        }
        // }else if (levelMap.CivilianBuildingGridPosisitions.gridPositionList...Contains(gridPosition))
        // {
        //     
        // }
    }

    private GridSlot CreateGridObject(GridManager gridManager, GridPosition gridPosition, GridPositionType slotType)
    {
        GridSlot newGridSlot = new GridSlot(gridPosition, null, slotType, true, null);
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




}