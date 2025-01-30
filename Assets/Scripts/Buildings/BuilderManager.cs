using System;
using Buildings;
using Buildings.CivilianBuildings;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BuilderManager : ISingleton<BuilderManager>
{

	[SerializeField] private GameObject civilianBuildingParentTransform;


	private void Start()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted += BuildCivilianBuildings;
		CivilianBuildingsUIManager.Instance.OnChoosingBuildingPlace += PlayerChoosingBuildingPlace;
	}

	private void OnDisable()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted -= BuildCivilianBuildings;
	}

	public void PlayerChoosingBuildingPlace(CivilianBuildingsSO civilianBuildingInfo)
	{
		try
		{
			GameObject civilianBuilding = Instantiate(civilianBuildingInfo.buildingPrefab,
				LevelGrid.Instance.positionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);

			BuildingConstructorSelector buildingConstructorSelector = civilianBuilding.GetComponent<BuildingConstructorSelector>();
			buildingConstructorSelector.ActivateBuildingConfirmOption(civilianBuildingInfo);
			//CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, civilianBuilding);
		}
		catch (Exception e)
		{
			Debug.LogError("There was an error choosing place for this building " + civilianBuildingInfo.buildingName);
			Debug.LogError(e.Message);
		}
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
			
			CivilianBuildingsManager.Instance.AddCivilianBuilding(LevelGrid.Instance.currentGridSlot.buildingID, civilianBuilding);
		}
		catch (Exception e)
		{
			Debug.LogError("There was an error building this place: " + civilianBuildingInfo.buildingName);
			Debug.LogError(e.Message);
		}

		
		//Hacer lo siguiente que toque
	}


}