using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager
{
    
    #region Private Fields
    
    public int width { get; private set; }
    public int height { get; private set; }
    private float cellSize;

    public GridSlot[,] grid { get; private set;}
    
    
    #endregion 	


    public GridManager(int width, int height, float cellSize)
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
                grid[x, y] = CreateGridObject(this, gridPosition);
            }
        }
        
    }

    private GridSlot CreateGridObject(GridManager gridManager, GridPosition gridPosition)
    {
        Debug.LogWarning("CREATING GRID IN POSITION: " + gridPosition.x + "," + gridPosition.y);
        GridSlot newGridSlot = new GridSlot(gridPosition, null, GridPositionType.Free, true, null);
        LevelGrid.Instance.InstantiateGridSlotPrefab(gridPosition);
        return newGridSlot;
    }
    



    #region Public Methods


    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0
               && gridPosition.y >= 0
               && gridPosition.x < width
               && gridPosition.y < height;
    }


    #endregion 	




}