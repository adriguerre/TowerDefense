using System;
using Buildings;
using Buildings.CivilianBuildings;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

public class BuilderManager : ISingleton<BuilderManager>
{

	[SerializeField] private GameObject civilianBuildingParentTransform;

	public GameObject _civilianBuildingPlaceholder;
	public CivilianBuildingsSO BuildInfo { get; private set; }

	private void Start()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted += BuildCivilianBuildings;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace += SpawnMoveableObjectSelector;
	}

	private void OnDisable()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted -= BuildCivilianBuildings;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace -= SpawnMoveableObjectSelector;

	}

	public void SpawnMoveableObjectSelector(CivilianBuildingsSO civilianBuildingInfo)
	{
		try
		{
			BuildInfo = civilianBuildingInfo;
			_civilianBuildingPlaceholder = Instantiate(civilianBuildingInfo.buildingPrefab,
				LevelGrid.Instance.positionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);

			BuildingConstructorSelector buildingConstructorSelector = _civilianBuildingPlaceholder.GetComponent<BuildingConstructorSelector>();
			buildingConstructorSelector.ActivateBuildingConfirmOption(civilianBuildingInfo);
			//CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, _civilianBuildingPlaceholder);
		}
		catch (Exception e)
		{
			Debug.LogError("There was an error choosing place for this building " + civilianBuildingInfo.buildingName);
			Debug.LogError(e.Message);
		}
	}

	public void MoveBuildingPlace(Vector2 position)
	{
		_civilianBuildingPlaceholder.transform.position = position;
	}


/// <summary>
	/// Method use to build civilian build
	/// We will instantiate object (start animation, etc..), save info in buildingsDicctionary
	/// </summary>
	/// <param name="civilianBuildingInfo"></param>
	public void BuildCivilianBuildings(CivilianBuildingsSO civilianBuildingInfo)
	{
		//Spawn building in position
		try
		{
			GameObject civilianBuilding = Instantiate(civilianBuildingInfo.buildingPrefab,
				LevelGrid.Instance.positionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);
			Debug.Log("CURRENT GRID SLOT: " + LevelGrid.Instance.currentGridSlot._gridPosition);
			LevelGrid.Instance.currentGridSlot.AddCivilianBuildingToAllSlot(civilianBuildingInfo);
			CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, civilianBuilding);
			CivilianBuildingsUIManager.Instance.playerIsChoosingPlaceToCivilianBuild = false;
		}
		catch (Exception e)
		{
			Debug.LogError("There was an error building this place: " + civilianBuildingInfo.buildingName);
			Debug.LogError(e.Message);
		}

		
		//Hacer lo siguiente que toque
	}


}