using System;
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

	public void BuildCivilianBuildings(CivilianBuildingsSO civilianBuildingInfo)
	{
		//Spawn building in position
		try
		{
			GameObject civilianBuilding = Instantiate(civilianBuildingInfo.buildingPrefab,
				LevelGrid.Instance.positionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);
		}
		catch (Exception e)
		{
			Debug.LogError("There was an error building this place: " + civilianBuildingInfo.buildingName);
		}

		
		//Hacer lo siguiente que toque
	}


}