using System.Collections.Generic;
using UnityEngine;

public class GridSlot
{
    #region Private Fields

    public GridPosition _gridPosition { get; private set; }
	IBuilding _buildingInPosition;
	List<GridPosition> _positionLinkedWithThisBuilding;
	bool _isInteractable; 
	public GridPositionType _gridPositionType { get; private set; }
	private GridManager _gridManager;
    #endregion

    public GridSlot(GridPosition gridPosition, IBuilding buildingInPosition, GridPositionType gridPositionType,
	    bool isInteractable, List<GridPosition> positionLinkedWithThisBuilding)
    {
	    this._gridPosition = gridPosition;
	    this._buildingInPosition = buildingInPosition;
	    this._gridPositionType = gridPositionType;
	    this._isInteractable = isInteractable;
	    this._positionLinkedWithThisBuilding = positionLinkedWithThisBuilding;
    }

    public void AddBuildingToSlot(IBuilding building)
    {
	    _buildingInPosition = building;
	    this._gridPositionType = GridPositionType.MilitaryBuilding;
	    _isInteractable = true;

    }


  /*  public void AddCivilianBuildingSpot(IBuilding building, List<GridPosition> positionLinkedWithThisBuilding)
    {
	    _buildingInPosition = null;
	    this._gridPositionType = GridPositionType.CivilianBuilding;
	    if (positionLinkedWithThisBuilding != null)
	    {
			this._positionLinkedWithThisBuilding = positionLinkedWithThisBuilding;
			List<GridPosition> newPositionLinkedWithThisBuilding = positionLinkedWithThisBuilding;
			
		    //TODO KW: Add the rest of the gridSlots the same buildings
		    foreach (var position in positionLinkedWithThisBuilding)
		    {
			    newPositionLinkedWithThisBuilding.Remove(position);
			    newPositionLinkedWithThisBuilding.Add(this._gridPosition);
			    _gridManager.LinkSlotToBuilding(building, this, position, positionLinked);
		    }
	    }
    }
    
    public void LinkSlotToOtherBuilding(IBuilding building, GridSlot parent, GridPosition position, List<GridPosition> positionLinked)
    {
	    GridSlot slotToModify = grid[position.x, position.y];
    }*/

}