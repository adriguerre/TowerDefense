using System;
using Buildings.CivilianBuildings;
using Unity.VisualScripting;
using UnityEngine;

public class BuilderManager : ISingleton<BuilderManager>
{

	[SerializeField] private GameObject civilianBuildingParentTransform;


	private void Start()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted += BuildCivilianBuildings;
	}

	private void OnDisable()
	{
		CivilianBuildingsUIManager.Instance.OnBuildingStarted -= BuildCivilianBuildings;
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
		}

		
		//Hacer lo siguiente que toque
	}


}