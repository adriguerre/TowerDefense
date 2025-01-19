using System;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
{

	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private float cellSize;
	[field: SerializeField] public GameObject gridDebugObjectPrefab {get; private set;}

	private GridManager gridSystem;

    private void Awake()
    {
	    gridSystem = new GridManager(width, height, cellSize);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Public Methods

    public void InstantiateGridSlotPrefab(GridPosition gridPosition)
    {

	    Instantiate(LevelGrid.Instance.gridDebugObjectPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
	    for(int x = 0; x < width; x++)
	    {
		    for(int y = 0; y < height; y++)
		    {
			    Gizmos.color = new Color(0, 0, 2, 0.5f);
			    Gizmos.DrawCube(GetWorldPosition(new GridPosition(x, y)), new Vector2(cellSize, cellSize));
		    }
	    }
    }

    #endregion 	

    public Vector2 GetWorldPosition(GridPosition gridPosition)
    {
	    return new Vector2(gridPosition.x, gridPosition.y) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
	    return new GridPosition(
		    Mathf.RoundToInt(worldPosition.x / cellSize),
		    Mathf.RoundToInt(worldPosition.z / cellSize));
    }
    
    #region Private Methods 
    #endregion 

    #region Getter & Setters 
    #endregion 	
	


}