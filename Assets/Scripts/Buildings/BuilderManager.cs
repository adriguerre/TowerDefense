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
		CivilianBuildingsUIManager.Instance.OnBuildingStarted += BuildCivilianBuildings;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace += SpawnCivilianMoveableObjectSelector;

		MilitaryBuildingsUIManager.Instance.OnChoosingBuildingPlace += SpawnMilitaryMoveableObjectSelector;
	}



	private void OnDisable()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted -= BuildCivilianBuildings;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace -= SpawnCivilianMoveableObjectSelector;
		
		MilitaryBuildingsUIManager.Instance.OnChoosingBuildingPlace -= SpawnMilitaryMoveableObjectSelector;


	}

	
	private void SpawnMilitaryMoveableObjectSelector(IBuildingsSO militaryBuildingInfo)
	{
		try
		{
			BuildInfo = militaryBuildingInfo;
			_BuildingPlaceholder = Instantiate(militaryBuildingInfo.buildingPrefab,
				LevelGrid.Instance.PositionToBuild, Quaternion.identity, militaryBuildingParentTransform.transform);
			CustomDebugger.Log(LogCategories.MilitaryBuildings, "POSITION TO BUILD: " + LevelGrid.Instance.PositionToBuild.ToString());

			BuildingConstructorSelector buildingConstructorSelector = _BuildingPlaceholder.GetComponent<BuildingConstructorSelector>();
			buildingConstructorSelector.ActivateBuildingConfirmOption(militaryBuildingInfo, false);
			//CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, _BuildingPlaceholder);
		}
		catch (Exception e)
		{
			CustomDebugger.LogError(LogCategories.MilitaryBuildings, "There was an error choosing place for this building " + militaryBuildingInfo.buildingName);
			Debug.LogError(e.Message);
		}
	}
	public void SpawnCivilianMoveableObjectSelector(IBuildingsSO civilianBuildingInfo)
	{
		try
		{
			BuildInfo = civilianBuildingInfo;
			_BuildingPlaceholder = Instantiate(civilianBuildingInfo.buildingPrefab,
				LevelGrid.Instance.PositionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);

			BuildingConstructorSelector buildingConstructorSelector = _BuildingPlaceholder.GetComponent<BuildingConstructorSelector>();
			buildingConstructorSelector.ActivateBuildingConfirmOption(civilianBuildingInfo, true);
			//CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, _BuildingPlaceholder);
		}
		catch (Exception e)
		{
			CustomDebugger.LogError(LogCategories.CivilianBuildings, "There was an error choosing place for this building " + civilianBuildingInfo.buildingName);

			Debug.LogError(e.Message);
		}
	}

	public void MoveBuildingPlace(Vector2 position)
	{
		_BuildingPlaceholder.transform.position = position;
	}


	/// <summary>
	/// Method use to build civilian build
	/// We will instantiate object (start animation, etc..), save info in buildingsDicctionary
	/// </summary>
	/// <param name="civilianBuildingInfo"></param>
	public void BuildCivilianBuildings(IBuildingsSO civilianBuildingInfo)
	{
		//Spawn building in position
		// try
		// {
			GameObject civilianBuilding = Instantiate(civilianBuildingInfo.buildingPrefab, 
				LevelGrid.Instance.PositionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);
			LevelGrid.Instance.currentGridSlot.AddCivilianBuildingToAllSlot(civilianBuildingInfo);
			CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, civilianBuilding);
			CivilianBuildingsUIManager.Instance.playerIsTryingToStartConstruction = false;
			NavigationManager.Instance.OpenScreenCanvas(TabTypes.Gameplay, false);
			civilianBuilding.GetComponent<CivilianBuilding>().Init(civilianBuildingInfo);
		// }
		// catch (Exception e)
		// {
		// 	Debug.LogError("There was an error building this place: " + civilianBuildingInfo.buildingName);
		// 	Debug.LogError(e.Message);
		// }

		
		//Hacer lo siguiente que toque
	}


}