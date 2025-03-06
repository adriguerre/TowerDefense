using System;
using Buildings;
using Buildings.CivilianBuildings;
using Buildings.MilitaryBuildings;
using BuildingsTest;
using CDebugger;
using MainNavBarUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuilderManager : ISingleton<BuilderManager>
{

	[SerializeField] private GameObject civilianBuildingParentTransform;
	[SerializeField] private GameObject militaryBuildingParentTransform;

	private GameObject _BuildingPlaceholder;
	public IBuildingsSO BuildInfo { get; private set; }

	private void Start()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted += StartBuildingConstruction;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace += SpawnMoveableObjectSelector;

		MilitaryBuildingsUIManager.Instance.OnChoosingBuildingPlace += SpawnMoveableObjectSelector;
	}



	private void OnDisable()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted -= StartBuildingConstruction;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace -= SpawnMoveableObjectSelector;
		MilitaryBuildingsUIManager.Instance.OnChoosingBuildingPlace -= SpawnMoveableObjectSelector;


	}

	
	private void SpawnMoveableObjectSelector(IBuildingsSO buildingInfo)
	{
		try
		{
			BuildInfo = buildingInfo;

			Transform parent = null;
			if (buildingInfo is MilitaryBuildingsSO)
			{
				parent = militaryBuildingParentTransform.transform;
				CustomDebugger.Log(LogCategories.MilitaryBuildings, "POSITION TO BUILD: " + LevelGrid.Instance.PositionToBuild.ToString());
			}
			else
			{
				parent = civilianBuildingParentTransform.transform;
			}
			_BuildingPlaceholder = Instantiate(buildingInfo.buildingPrefab,
				LevelGrid.Instance.PositionToBuild, Quaternion.identity, parent);
		

			
			BuildingConstructorSelector buildingConstructorSelector = _BuildingPlaceholder.GetComponent<BuildingConstructorSelector>();
			buildingConstructorSelector.ActivateBuildingConfirmOption(buildingInfo);
			//CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, _BuildingPlaceholder);
		}
		catch (Exception e)
		{
			CustomDebugger.LogError(LogCategories.Buildings, "There was an error choosing place for this building " + buildingInfo.buildingName);
			Debug.LogError(e.Message);
		}
	}

	public void MoveBuildingPlace(Vector2 position)
	{
		_BuildingPlaceholder.transform.position = position;
	}
	
	/// <summary>
	/// Start building
	/// It will detect inside if it is a civilian or military building
	/// We will instantiate object (start animation, etc..), save info in buildingsDicctionary
	/// </summary>
	/// <param name="buildingInfo"></param>
	public void StartBuildingConstruction(IBuildingsSO buildingInfo)
	{
		//Spawn building in position
		// try
		// {
		bool isCivilianBuilding = buildingInfo is CivilianBuildingsSO;
		
			GameObject building = Instantiate(buildingInfo.buildingPrefab, 
				LevelGrid.Instance.PositionToBuild, Quaternion.identity, 
				isCivilianBuilding ? civilianBuildingParentTransform.transform : militaryBuildingParentTransform.transform);
			if (isCivilianBuilding)
			{
				LevelGrid.Instance.currentGridSlot.AddCivilianBuildingToAllSlot(buildingInfo);
				CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, building);
				CivilianBuildingsUIManager.Instance.playerIsTryingToStartConstruction = false;
			}
			else
			{
				Vector2 positionToBuild = LevelGrid.Instance.PositionToBuild;
				GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(positionToBuild);
				LevelGrid.Instance.ChangeGridSlotTypeToMilitary(gridPosition);
				MilitaryBuildingsManager.Instance.AddMilitaryBuildingsPosition(gridPosition, building);
				MilitaryBuildingsUIManager.Instance.playerIsTryingToStartConstruction = false;
			}
			
			building.GetComponent<CivilianBuilding>().Init(buildingInfo);
			NavigationManager.Instance.OpenScreenCanvas(TabTypes.Gameplay, false);
		// }
		// catch (Exception e)
		// {
		// 	Debug.LogError("There was an error building this place: " + buildingInfo.buildingName);
		// 	Debug.LogError(e.Message);
		// }
	}


}