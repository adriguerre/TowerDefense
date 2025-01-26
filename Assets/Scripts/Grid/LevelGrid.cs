using System;
using System.Collections.Generic;
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
			Debug.Log("SE esta creando un level con la info de: " + levelSO.levelName + " con un camino count de: " + levelSO.pathList.Count);
			gridSystem = new GridManager(width, height, cellSize, levelSO);
		}
		else
		{
			LevelSO defaultLevel = Resources.Load<LevelSO>("Levels/LeveL_1");
			gridSystem = new GridManager(width, height, cellSize, defaultLevel);
		}
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
	    if (Input.GetMouseButtonDown(0))
	    {
		    GridSlot gridSlot = gridSystem.GetGridSlotFromMousePosition();
		    if (gridSlot != null && currentGridSlot != gridSlot)
		    {
			    bool isBuilding = false;
			    if (gridSlot._gridPositionType == GridPositionType.CivilianBuilding)
			    {
				    Debug.Log("WE ARE CLICKING CIVILIAN BUILDING WITH ID: " + gridSlot.buildingID);
				    isBuilding = true;
			    }
			    ActivateGridSlotBuildingUI(gridSlot, isBuilding);
			    CheckIfLevelCreatorIsEnabled(gridSlot);
		    }
		 
	    }
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
				    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridDebugCivilianBuildingSize4ObjectPrefab, GetWorldPosition(gridSlot._gridPosition), Quaternion.identity); 
			    else
				    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridDebugCivilianBuildingSize6ObjectPrefab, GetWorldPosition(gridSlot._gridPosition), Quaternion.identity); 
		    } 
	    }
	    else
		    currentGridBuildingUI = Instantiate(LevelGrid.Instance.gridDebugObjectPrefab, GetWorldPosition(gridSlot._gridPosition), Quaternion.identity); 
	    
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