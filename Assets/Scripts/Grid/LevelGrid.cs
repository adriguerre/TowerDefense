using System;
using System.Net;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
{

	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private float cellSize;
	[field: SerializeField] public GameObject gridDebugObjectPrefab {get; private set;}

	private GridManager gridSystem;
	private GameObject currentGridBuildingUI;
	GridSlot currentGridSlot;

    private void Awake()
    {
	    gridSystem = new GridManager(width, height, cellSize);
    }
    void Start()
    {
        
    }

    void Update()
    {
	    if (CameraScroll.Instance.isMovingCamera)
	    {
		    return;
	    }
	    
	    if (Input.GetMouseButtonDown(0))
	    {
		    GridSlot gridSlot = gridSystem.GetGridSlotFromMousePosition();
		    if (gridSlot != null && currentGridSlot != gridSlot)
		    {
			    ActivateGridSlotBuildingUI(gridSlot); 
		    }
		 
	    }
    }

    private void ActivateGridSlotBuildingUI(GridSlot gridSlot)
    {
	    if (currentGridBuildingUI != null)
	    {
		    Destroy(currentGridBuildingUI);
	    }
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