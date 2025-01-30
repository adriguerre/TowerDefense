using System.Collections.Generic;
using UnityEngine;

public class GridSlot
{
    #region Private Fields
    public GridPosition _gridPosition { get; private set; }
	public GridPositionType _gridPositionType { get; set; }
	private GridManager _gridManager;
	//This can be 0 if no building is there
	public int buildingID {get; private set;}
	//This can be 0 if no building is there
	public int buildingSize {get; private set;}
	
	public CivilianBuildingsSO civilianBuildingSO {get; private set;}
	
    #endregion

    public GridSlot(GridPosition gridPosition, CivilianBuildingsSO civilianBuildingInPosition, GridPositionType gridPositionType, int buildingID, int buildingSize)
    {
	    this._gridPosition = gridPosition;
	    this._gridPositionType = gridPositionType;
	    this.civilianBuildingSO = civilianBuildingInPosition;
	    this.buildingID = buildingID;
	    this.buildingSize = buildingSize;
    }

    public void AddCivilianBuildingToAllSlot(CivilianBuildingsSO building)
    {
	    //This should be linked to all the linked positions
	    LevelGrid.Instance.LinkGridSlotsToBuilding(building, this);
	    civilianBuildingSO = building;
    }
    
    public void AddCivilianBuildingToOneSlot(CivilianBuildingsSO building)
    {
	    //This should be linked to all the linked positions
	    civilianBuildingSO = building;
    }

    public CivilianBuildingsSO GetBuildingInGridSlot()
    {
	    return civilianBuildingSO;
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