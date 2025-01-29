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
		GameObject civilianBuilding = Instantiate(civilianBuildingInfo.buildingPrefab, 
			LevelGrid.Instance.positionToBuild, Quaternion.identity, civilianBuildingParentTransform.transform);
		
		//Hacer lo siguiente que toque
	}


}