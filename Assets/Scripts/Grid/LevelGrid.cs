using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
	[field: SerializeField] public GameObject gridDebugObjectPrefab {get; private set;}
	[field: SerializeField] public GameObject gridDebugCivilianBuildingSize4ObjectPrefab {get; private set;}
	[field: SerializeField] public GameObject gridDebugCivilianBuildingSize6ObjectPrefab {get; private set;}
	

	[SerializeField] private LevelSO defaultLevel;
	private GridManager gridSystem;
	private GameObject currentGridBuildingUI;
	GridSlot currentGridSlot;
	public Vector2 positionToBuild;
	private LevelSO currentLevelSO;

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

	private void Start()
	{
	}

	public async Task CreateLevel(LevelSO levelSO)
	{
		if (levelSO != null)
		{
			currentLevelSO = levelSO;
			Debug.Log("SE esta creando un level con la info de: " + levelSO.levelName + " con un camino count de: " + levelSO.pathList.Count);
			gridSystem = new GridManager(width, height, cellSize, levelSO);
		}
		else
		{
			LevelSO defaultLevel = Resources.Load<LevelSO>("Levels/LeveL_1");
			currentLevelSO = defaultLevel;
			gridSystem = new GridManager(width, height, cellSize, defaultLevel);
		}
	}

	public Vector2 GetCenterPositionFromCivilianBuilding(int buildingID, int size)
	{
		foreach (var CivilianBuilding in currentLevelSO.CivilianBuildingGridPosisitions)
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

	void Update()
    {
	    if (CameraScroll.Instance != null)
	    {
		    if (CameraScroll.Instance.isMovingCamera)
		    {
			    return;
		    } 
	    }
	   
    }

	public void ClickOnLevelGrid()
	{
		GridSlot gridSlot = gridSystem.GetGridSlotFromMousePosition();
		if (gridSlot != null && currentGridSlot != gridSlot)
		{
			currentGridSlot = gridSlot;
			bool isBuilding = false;
			if (gridSlot._gridPositionType == GridPositionType.CivilianBuilding)
			{
				Debug.Log("WE ARE CLICKING CIVILIAN BUILDING WITH ID: " + currentGridSlot.buildingID);
				Debug.Log("WE ARE CLICKING CIVILIAN BUILDING WITH SIZE: " + currentGridSlot.buildingSize);
				isBuilding = true;
			}
			ActivateGridSlotBuildingUI(currentGridSlot, isBuilding);
			CheckIfLevelCreatorIsEnabled(currentGridSlot);
		}else if (currentGridSlot == gridSlot)
		{
			DesactivateGridSlotPrefab();
		}
	}

	public void DesactivateGridSlotPrefab()
	{
		if (currentGridBuildingUI != null)
			Destroy(currentGridBuildingUI);

		if (currentGridSlot != null)
		{
			if (currentGridSlot._gridPositionType == GridPositionType.CivilianBuilding)
			{
				CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
			}
		}
		currentGridSlot = null;

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

	private void ActivateGridSlotBuildingUI(GridSlot gridSlot, bool isBuilding)
    {
	    if (currentGridBuildingUI != null)
		    Destroy(currentGridBuildingUI);
	    //If it is a building, we spawn 4 grid size object or 6 grid size
	    if (isBuilding)
	    {
		    if (gridSlot.buildingSize != 0)
		    {
			    if (gridSlot.buildingSize == 4)
			    {
				    Vector2 position = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID, 4);
				    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridDebugCivilianBuildingSize4ObjectPrefab, position, Quaternion.identity); 
				    CivilianBuildingsUIPopButtons.Instance.OpenBuildUI(position, currentGridSlot.buildingSize);
				    positionToBuild = position;
			    }
			    else
			    {
				    Vector2 position = GetCenterPositionFromCivilianBuilding(gridSlot.buildingID, 6);
				    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridDebugCivilianBuildingSize6ObjectPrefab, position, Quaternion.identity); 
				    CivilianBuildingsUIPopButtons.Instance.OpenBuildUI(position, currentGridSlot.buildingSize);
				    positionToBuild = position;
			    }
		    } 
	    }
	    else
	    {
		    CivilianBuildingsUIPopButtons.Instance.CloseBuildUI();
		    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridDebugObjectPrefab, GetWorldPosition(gridSlot._gridPosition), Quaternion.identity);
		    //TODO KW
		    positionToBuild = GetWorldPosition(gridSlot._gridPosition);
	    }
 
	    
	    currentGridBuildingUI.GetComponent<GridSlotHolder>().SetProperties(gridSlot);
    }

    #region Public Methods

    public void InstantiateGridSlotPrefab(GridPosition gridPosition)
    {
	  // Instantiate(LevelGrid.Instance.gridDebugObjectPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
    }

    

    private void OnDrawGizmos()
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
    
    #region Private Methods 
    #endregion 

    #region Getter & Setters 
    #endregion 	
	


}