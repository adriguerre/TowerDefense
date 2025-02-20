using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Buildings.CivilianBuildings;
using Buildings.MilitaryBuildings;
using BuildingsTest;
using CDebugger;
using NaughtyAttributes;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class LevelGrid : Singleton<LevelGrid>
{

	[Header("Properties")]
	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private float cellSize;
	[field: SerializeField] public GameObject gridObserverObjectPrefab {get; private set;}
	[field: SerializeField] public GameObject gridObserverCivilianBuildingSize4ObjectPrefab {get; private set;}
	[field: SerializeField] public GameObject gridObserverCivilianBuildingSize6ObjectPrefab {get; private set;}
	

	[SerializeField] private LevelSO defaultLevel;
	private GridManager gridSystem;
	private GameObject currentGridBuildingUI;
	private List<GameObject> selectorGridBuildingsObjects;
	public GridSlot currentGridSlot { get; private set; }
	public Vector2 positionToBuild;
	public LevelSO CurrentLevelSO { get; private set; }


	#region Level Creator

	[Header("Level Creator")]
	[SerializeField] private bool LEVEL_CREATOR;
	[EnableIf(EConditionOperator.And, "LEVEL_CREATOR", "NotLC_buildingObstacle", "NotLC_buildingTemporaryObstacles", "NotLC_buildingBuildings")]
	[SerializeField] private bool LC_buildingPaths;
	[EnableIf(EConditionOperator.And, "LEVEL_CREATOR", "NotLC_buildingPaths", "NotLC_buildingTemporaryObstacles", "NotLC_buildingBuildings")]
	[SerializeField] private bool LC_buildingObstacle;
	[EnableIf(EConditionOperator.And, "LEVEL_CREATOR", "NotLC_buildingObstacle", "NotLC_buildingPaths", "NotLC_buildingBuildings")]
	[SerializeField] private bool LC_buildingTemporaryObstacles;
	[EnableIf(EConditionOperator.And, "LEVEL_CREATOR", "NotLC_buildingObstacle", "NotLC_buildingPaths", "NotLC_buildingTemporaryObstacles")]
	[SerializeField] private bool LC_buildingBuildings;
	[EnableIf("LEVEL_CREATOR")]
	[SerializeField] private string FileName;
	public bool levelIsCreated { get; private set; }
	private List<GridPosition> pathsSlots;
	private List<GridPosition> obstacleSlots;
	List<GridPosition> temporaryObstacleSlots;
	List<GridPosition> civilianBuildingsSlots;
	[Button]
	private void SaveLevel()
	{
		LevelSO levelToModify = Resources.Load<LevelSO>("Levels/" + FileName);

		if (levelToModify != null)
		{
			Debug.Log("LEVEL SAVED");
			levelToModify.pathList.Clear();
			levelToModify.pathList = pathsSlots;
		}
		else
		{
			Debug.Log("LEVEL NOT SAVED");
		}
	}
	#endregion
	

	private void Start()
	{
		selectorGridBuildingsObjects = new List<GameObject>();
	}

	private void Update()
	{
		// if(currentGridSlot != null)
		// 	Debug.Log("KW: GRID SLOT: " + currentGridSlot._gridPosition);
		// else
		// 	Debug.Log("KW: NO GRID SLOT");
	}

	public async Task CreateLevel(LevelSO levelSO)
	{
		if (levelSO != null)
		{
			CurrentLevelSO = levelSO;
			Debug.Log("SE esta creando un level con la info de: " + levelSO.levelName + " con un camino count de: " + levelSO.pathList.Count);
			gridSystem = new GridManager(width, height, cellSize, levelSO);
		}
		else
		{
			LevelSO defaultLevel = Resources.Load<LevelSO>("Levels/LeveL_1");
			CurrentLevelSO = defaultLevel;
			gridSystem = new GridManager(width, height, cellSize, defaultLevel);
		}
		CivilianBuildingsManager.Instance.FillCivilianBuildingsDictionary(CurrentLevelSO);
	}

	/// <summary>
	/// Get position where is supposed to be the center (for 4 and 6 grid size)
	/// </summary>
	/// <param name="buildingID"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	public Vector2 GetCenterPositionFromCivilianBuilding(int buildingID, int size)
	{
		foreach (var CivilianBuilding in CurrentLevelSO.CivilianBuildingGridPosisitions)
		{
			if (CivilianBuilding.buildingId == buildingID)
			{
				if (size == 4)
				{
					GridPosition gridPosition = CivilianBuilding.gridPositionList.First();
					return gridSystem.GetTopRightCorner(gridPosition);
				}else if (size == 6)
				{
					GridPosition gridPosition = CivilianBuilding.gridPositionList[1];
					return gridSystem.GetTopPosition(gridPosition);
				}
			}
		}

		return Vector2.zero;
	}

	/// <summary>
	/// Set grid slot from a given position
	/// </summary>
	/// <param name="position"></param>
	public void SetCurrentGridSlotFromWorldPosition(Vector2 position)
	{
		GridPosition gridPosition = GetGridPosition(position);
		currentGridSlot = gridSystem.GetGridSlotFromGridPosition(gridPosition);
	}
	
	

	public void ClickOnLevelGrid()
	{
		GridSlot gridSlot = gridSystem.GetGridSlotFromMousePosition();
		if (gridSlot != null && currentGridSlot != gridSlot)
		{
			currentGridSlot = gridSlot;
			bool isBuilding = false;
			bool isMilitaryBuilding = false;
			if (gridSlot._gridPositionType == GridPositionType.CivilianBuilding)
			{
				Debug.Log("WE ARE CLICKING CIVILIAN BUILDING WITH ID: " + currentGridSlot.buildingID);
				if (currentGridSlot.GetBuildingInGridSlot() != null)
				{
					Debug.Log("THERE IS ALREADY A BUILDING IN THIS LOCATION:" + currentGridSlot.GetBuildingInGridSlot().buildingName.ToString());
				}
				isBuilding = true;
			}else if (gridSlot._gridPositionType == GridPositionType.MilitaryBuilding)
			{
				isMilitaryBuilding = true;
			}
			ActivateGridSlotBuildingUI(currentGridSlot, isBuilding, true,isMilitaryBuilding);
			CheckIfLevelCreatorIsEnabled(currentGridSlot);
		}else if (currentGridSlot == gridSlot)
		{
			DesactivateGridSlotPrefabAndHideBuildUIPop();
		}
	}

	public void SelectSlotAsPossibleCivilianLocation()
	{
		GridSlot gridSlot = gridSystem.GetGridSlotFromMousePosition();
		Debug.Log(gridSlot._gridPosition);
		if (gridSlot != null && currentGridSlot != gridSlot)
		{
			if (gridSlot._gridPositionType == GridPositionType.CivilianBuilding)
			{
				if (gridSlot.buildingSize < BuilderManager.Instance.BuildInfo.buildSize)
				{
					return;
				}

				currentGridSlot = gridSlot;
				if (currentGridSlot.GetBuildingInGridSlot() == null)
				{
					positionToBuild = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID,  BuilderManager.Instance.BuildInfo.buildSize);
					currentGridSlot = gridSlot;
					ActivateGridSlotBuildingUI(currentGridSlot, true, false, false);
					BuilderManager.Instance.MoveBuildingPlace(positionToBuild);
				}
			}
		}
	}
	
	public void SelectSlotAsPossibleMilitaryLocation()
	{
		GridSlot gridSlot = gridSystem.GetGridSlotFromMousePosition();
		Debug.Log(gridSlot._gridPosition);
		if (gridSlot != null && currentGridSlot != gridSlot)
		{
			if (IsNotValidPositionForMilitaryBuilding(gridSlot))
			{
				return;
			}
			currentGridSlot = gridSlot;
			if (currentGridSlot.GetBuildingInGridSlot() == null)
			{
				positionToBuild = new Vector2(gridSlot._gridPosition.x, gridSlot._gridPosition.y);
				currentGridSlot = gridSlot;
				ActivateGridSlotBuildingUI(currentGridSlot, false, false, true);
				BuilderManager.Instance.MoveBuildingPlace(positionToBuild);
			}
			
		}
	}

	public bool IsNotValidPositionForMilitaryBuilding(GridSlot gridSlot)
	{
		return (gridSlot._gridPositionType == GridPositionType.Obstacle || 
		        gridSlot._gridPositionType == GridPositionType.CivilianBuilding ||
		        gridSlot._gridPositionType == GridPositionType.Path ||
		        gridSlot._gridPositionType == GridPositionType.TemporaryObstacle);
	}
	public void DesactivateGridSlotPrefabAndHideBuildUIPop()
	{
		if (currentGridBuildingUI != null)
			Destroy(currentGridBuildingUI);

		if (currentGridSlot != null)
		{
			if (currentGridSlot._gridPositionType == GridPositionType.CivilianBuilding)
			{
				BuildingsUIPopButtons.Instance.CloseBuildUI();
			}
		}
		currentGridSlot = null;
	}

	public void DestroyGridBuildPrefab()
	{
		if (currentGridBuildingUI != null)
			Destroy(currentGridBuildingUI);
		currentGridSlot = null;
	}
	
	/// <summary>
	/// Given a gridslot, we link the rest of the civilian building the same civilian build id
	/// </summary>
	/// <param name="building"></param>
	/// <param name="gridSlotParent"></param>
	public void LinkGridSlotsToBuilding(IBuildingsSO building, GridSlot gridSlotParent)
	{
		foreach (var civilianBuildingsPosition in CurrentLevelSO.CivilianBuildingGridPosisitions)
		{
			if (civilianBuildingsPosition.gridPositionList.Contains(gridSlotParent._gridPosition))
			{
				foreach (var gridPosition in civilianBuildingsPosition.gridPositionList)
				{
					if (gridSlotParent._gridPosition != gridPosition)
					{
						GridSlot slot = gridSystem.GetGridSlotFromGridPosition(gridPosition);
						slot.AddCivilianBuildingToOneSlot(building);
					}
				}
				return;
			}
		}
	}

	public void UnlinkBuildingFromAllCloseSlots(GridSlot gridSlotParent)
	{
		foreach (var civilianBuildingsPosition in CurrentLevelSO.CivilianBuildingGridPosisitions)
		{
			if (civilianBuildingsPosition.gridPositionList.Contains(gridSlotParent._gridPosition))
			{
				foreach (var gridPosition in civilianBuildingsPosition.gridPositionList)
				{
					if (gridSlotParent._gridPosition != gridPosition)
					{
						GridSlot slot = gridSystem.GetGridSlotFromGridPosition(gridPosition);
						slot.RemoveCivilianBuildingFromSlot();
					}
				}
				return;
			}
		}
	}
	
	public CivilianBuildingGridPosition GetClosestCivilianBuildingToMousePositionAndActivateGrid(int size)
	{
		CivilianBuildingGridPosition closestBuildingObject = null;
		GridPosition closestPosition = gridSystem.GetGridSlotFromMousePosition()._gridPosition;
		double minDistance = 100;
		foreach (var civilianBuilding in CurrentLevelSO.CivilianBuildingGridPosisitions)
		{
			if (civilianBuilding.size >= size)
			{
				foreach (var gridPosition in civilianBuilding.gridPositionList)
				{
					if (!CivilianBuildingsManager.Instance.CurrentBuildingsDictionary.ContainsKey(civilianBuilding
						    .buildingId)) //Make sure this spot is not take by other building
					{
						if (gridPosition.DistanceTo(closestPosition) < minDistance)
						{
							minDistance = gridPosition.DistanceTo(closestPosition);
							closestBuildingObject = civilianBuilding;
							closestPosition = gridPosition; //Check if can remove
						}
					}
				}
			}
		}
		if (closestBuildingObject != null)
		{
			GridSlot gridSlot = gridSystem.GetGridSlotFromGridPosition(closestBuildingObject.gridPositionList.First());
			ActivateGridSlotBuildingUI(gridSlot, true, false, false);
			if (size == 6)
				positionToBuild = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID, 6);
			else
				positionToBuild = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID, 4);

		}
		return closestBuildingObject;
	}
	
	public GridSlot GetClosestAvailablePositionToMilitaryBuilding()
	{
		GridSlot closestBuildingObject = null;
		GridPosition position = new GridPosition((int)Camera.main.gameObject.transform.position.x,
			(int)Camera.main.gameObject.transform.position.y);
		CustomDebugger.Log(LogCategories.MilitaryBuildings, position.ToString());
		GridPosition closestPosition = new GridPosition((int) Camera.main.gameObject.transform.position.x, (int) Camera.main.gameObject.transform.position.y);
		GridPosition gridPosition = null;
		double minDistance = 100;

		for (int i = 0; i < gridSystem.width; i++)
		{
			for (int j = 0; j < gridSystem.height; j++)
			{
				gridPosition = new GridPosition(i, j);
				//Este gridslot no se asigna bien, por lo tanto es siempre 0 0
				GridSlot gridSlot = gridSystem.GetGridSlotFromGridPosition(gridPosition);
				if (gridSlot == null || gridSlot.IsRoad || 
				    MilitaryBuildingsManager.Instance.CurrentBuildingsDictionary.ContainsKey(gridPosition)
				    || IsNotValidPositionForMilitaryBuilding(gridSlot) )
				{
					continue;
				}
				
				if (gridPosition.DistanceTo(position) < minDistance)
				{
					minDistance = gridPosition.DistanceTo(closestPosition);
					closestBuildingObject = gridSlot;
				}
			}
		}

		if (closestBuildingObject != null)
		{
			ActivateGridSlotBuildingUI(closestBuildingObject, false, false, true);
			positionToBuild = GetWorldPosition(closestBuildingObject._gridPosition);
		}
		
		return closestBuildingObject;
	}
	
	private void CheckIfLevelCreatorIsEnabled(GridSlot gridSlot)
	{

		if (!LEVEL_CREATOR)
		{
			return;
		}
		if (pathsSlots == null) 
			pathsSlots = new List<GridPosition>();
		if (obstacleSlots == null) 
			obstacleSlots = new List<GridPosition>();
		if (temporaryObstacleSlots == null) 
			temporaryObstacleSlots = new List<GridPosition>();
		if (civilianBuildingsSlots == null) 
			civilianBuildingsSlots = new List<GridPosition>();
		
		
		if (LC_buildingPaths)
		{
			pathsSlots.Add(gridSlot._gridPosition);
			gridSlot._gridPositionType = GridPositionType.Path;
		}
		
		if (LC_buildingObstacle)
		{
			obstacleSlots.Add(gridSlot._gridPosition);
			gridSlot._gridPositionType = GridPositionType.Obstacle;

		}
		
		if (LC_buildingTemporaryObstacles)
		{
			temporaryObstacleSlots.Add(gridSlot._gridPosition);
			gridSlot._gridPositionType = GridPositionType.TemporaryObstacle;

		}
		
		if (LC_buildingBuildings)
		{
			civilianBuildingsSlots.Add(gridSlot._gridPosition);
			gridSlot._gridPositionType = GridPositionType.CivilianBuilding;

		}
	}

	/// <summary>
	/// Activate grid slot selector placeholder for a given grid slot
	/// </summary>
	/// <param name="gridSlot"></param>
	/// <param name="isBuilding"></param>
	/// <param name="showBuildUIPopup"></param>
	public void ActivateGridSlotBuildingUI(GridSlot gridSlot, bool isBuilding, bool showBuildUIPopup,bool isMilitaryBuilding)
    {
	    if (currentGridBuildingUI != null)
		    Destroy(currentGridBuildingUI);
	    //If it is a building, we spawn 4 grid size object or 6 grid size
	    if (isBuilding)
	    {
		    ActivateCivilianBuildingObserver(gridSlot, showBuildUIPopup);
	    }
	    else if (isMilitaryBuilding)
	    {
		    Vector2 position = new Vector2(gridSlot._gridPosition.x, gridSlot._gridPosition.y);
		    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridObserverObjectPrefab, GetWorldPosition(gridSlot._gridPosition), Quaternion.identity); 
		    if(showBuildUIPopup)
			    BuildingsUIPopButtons.Instance.OpenBuildUI(position, currentGridSlot);
		    positionToBuild = GetWorldPosition(gridSlot._gridPosition);
	    }
	    else
	    {
		    BuildingsUIPopButtons.Instance.CloseBuildUI();
		    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridObserverObjectPrefab, GetWorldPosition(gridSlot._gridPosition), Quaternion.identity);
		    //TODO KW
		    positionToBuild = GetWorldPosition(gridSlot._gridPosition);
	    }
	    currentGridBuildingUI.GetComponent<GridSlotHolder>().SetProperties(gridSlot);
    }

	private void ActivateCivilianBuildingObserver(GridSlot gridSlot, bool showBuildUIPopup)
	{
		if (gridSlot.buildingSize == 0)
			return;
		
		
		if (gridSlot.buildingSize == 4)
		{
			Vector2 position = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID, 4);
			currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridObserverCivilianBuildingSize4ObjectPrefab, position, Quaternion.identity); 
			if(showBuildUIPopup)
				BuildingsUIPopButtons.Instance.OpenBuildUI(position, currentGridSlot);
			positionToBuild = position;
		}
		else
		{
			Vector2 position = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID, 6);
			currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridObserverCivilianBuildingSize6ObjectPrefab, position, Quaternion.identity); 
			if(showBuildUIPopup)
				BuildingsUIPopButtons.Instance.OpenBuildUI(position, currentGridSlot);
			positionToBuild = position;
		}
	}

    #region Public Methods

    public void InstantiateGridSlotPrefab(GridPosition gridPosition)
    {
	  // Instantiate(LevelGrid.Instance.gridObserverObjectPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
    }
    
    private void OnDrawGizmos()
    {
	    if(Application.isPlaying)
	    {
		    if (LEVEL_CREATOR)
		    {
			    DrawLevelCreatorGizmos();
		    }
		    else
		    {
			    DrawNormalGizmos();
		    }
	    }
    }

    private void DrawLevelCreatorGizmos()
    {
	    for(int x = 0; x < width; x++)
	    {
		    for(int y = 0; y < height; y++)
		    {
			    GridSlot gridSlot = gridSystem.GetGridSlotFromGridPosition(new GridPosition(x, y));
			    switch (gridSlot._gridPositionType)
			    {
				    case GridPositionType.Path:
					    Gizmos.color = new Color(1, 1, 0, 0.5f);
					    break;
				    case GridPositionType.Free:
					    Gizmos.color = new Color(0, 1, 0, 0.5f);
					    break;
				    case GridPositionType.Obstacle:
					    Gizmos.color = new Color(0.7f, 0.8f, 0.1f, 0.5f);
					    break;
				    case GridPositionType.CivilianBuilding:
					    Gizmos.color = new Color(0.7f, 0.1f, 0.7f, 0.5f);
					    break;
				    case GridPositionType.MilitaryBuilding:
					    Gizmos.color = new Color(0.2f, 0.1f, 0.7f, 0.5f);
					    break;
				    case GridPositionType.TemporaryObstacle:
					    Gizmos.color = new Color(0.2f, 0.1f, 0.2f, 0.5f);
					    break;
				    default:
					    Gizmos.color = new Color(1, 1, 1, 0.5f);
					    break;
			    }
			    Gizmos.DrawCube(GetWorldPosition(new GridPosition(x, y)), new Vector2(cellSize, cellSize));
		    }
	    }
    }

    private void DrawNormalGizmos()
    {
	    for(int x = 0; x < width; x++)
	    {
		    for(int y = 0; y < height; y++)
		    {
			    GridSlot gridSlot = gridSystem.GetGridSlotFromGridPosition(new GridPosition(x, y));
			    switch (gridSlot._gridPositionType)
			    {
				    case GridPositionType.Path:
					    Gizmos.color = new Color(1, 1, 0, 0.5f);
					    break;
				    case GridPositionType.Free:
					    Gizmos.color = new Color(0, 1, 0, 0.5f);
					    break;
				    case GridPositionType.Obstacle:
					    Gizmos.color = new Color(0.7f, 0.8f, 0.1f, 0.5f);
					    break;
				    case GridPositionType.CivilianBuilding:
					    Gizmos.color = new Color(0.7f, 0.1f, 0.7f, 0.5f);
					    break;
				    case GridPositionType.MilitaryBuilding:
					    Gizmos.color = new Color(0.2f, 0.1f, 0.7f, 0.5f);
					    break;
				    case GridPositionType.TemporaryObstacle:
					    Gizmos.color = new Color(0.2f, 0.1f, 0.2f, 0.5f);
					    break;
				    default:
					    Gizmos.color = new Color(1, 1, 1, 0.5f);
					    break;
			    }
			    Gizmos.DrawCube(GetWorldPosition(new GridPosition(x, y)), new Vector2(cellSize, cellSize));
		    }
	    }
    }

    #endregion 	

    public Vector2 GetWorldPosition(GridPosition gridPosition)
    {
	    return new Vector2(gridPosition.x, gridPosition.y) * cellSize;
    }
    
    public GridPosition GetGridPosition(Vector2 worldPosition)
    {
	    return new GridPosition(
		    Mathf.RoundToInt(worldPosition.x / cellSize),
		    Mathf.RoundToInt(worldPosition.y / cellSize));
    }

    public int GetSizeFromBuildingID(int buildingID)
    {
	    return CurrentLevelSO.CivilianBuildingGridPosisitions.Find(t => t.buildingId == buildingID).size;
    }
	


}