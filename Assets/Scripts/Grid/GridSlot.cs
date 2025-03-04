using System.Collections.Generic;
using BuildingsTest;
using UnityEngine;

public class GridSlot
{
	
	//This can be 0 if no building is there
	public int buildingID {get; private set;}
	//This can be 0 if no building is there
	public int buildingSize {get; private set;}
	
	public IBuildingsSO BuildingSO {get; private set;}
	public bool IsRoad { get; private set; }
    public GridPosition _gridPosition { get; private set; }
	public GridPositionType _gridPositionType { get; set; }
	private GridManager _gridManager;
	
    public GridSlot(GridPosition gridPosition, CivilianBuildingsSO buildingInPosition, GridPositionType gridPositionType, int buildingID, int buildingSize)
    {
	    this._gridPosition = gridPosition;
	    this._gridPositionType = gridPositionType;
	    this.BuildingSO = buildingInPosition;
	    this.buildingID = buildingID;
	    this.buildingSize = buildingSize;
    }

    public void AddCivilianBuildingToAllSlot(IBuildingsSO building)
    {
	    //This should be linked to all the linked positions
	    LevelGrid.Instance.LinkGridSlotsToBuilding(building, this);
	    BuildingSO = building;
    }
    
    public void AddCivilianBuildingToOneSlot(IBuildingsSO building)
    {
	    //This should be linked to all the linked positions
	    BuildingSO = building;
    }

    public void RemoveCivilianBuildingFromAllSlots()
    {
	    //This should be linked to all the linked positions
	    LevelGrid.Instance.UnlinkBuildingFromAllCloseSlots(this);
	    BuildingSO = null;
    }

    public void RemoveCivilianBuildingFromSlot()
    {
	    BuildingSO = null;
    }

    public IBuildingsSO GetBuildingInGridSlot()
    {
	    return BuildingSO;
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