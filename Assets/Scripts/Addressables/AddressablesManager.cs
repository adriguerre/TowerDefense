using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingsTest;
using CDebugger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.PlayerLoop;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressablesManager
{
	public class AddressablesManager : MonoBehaviour
	{
		[SerializeField] private AssetLabelReference _civilianBuildingAddressablesLabel;
		[SerializeField] private AssetLabelReference _militaryBuildingAddressablesLabel;
		
		public static event EventHandler<List<IBuildingsSO>> OnCivilianBuildingsLoaded; 
		public static event EventHandler<List<IBuildingsSO>> OnMilitaryBuildingsLoaded; 
		
		private async void Start()
		{
			await Init();
			
			CustomDebugger.Log(LogCategories.Addressables, "Addressables Manager initialized");
			CustomDebugger.Log(LogCategories.Addressables, "Comunicating to loading manager");
		}

		private async Task Init()
		{
			Addressables.LoadAssetsAsync<IBuildingsSO>(_civilianBuildingAddressablesLabel, so => {}).Completed +=
				OnCivilianInfoReceived;
			
			Addressables.LoadAssetsAsync<IBuildingsSO>(_militaryBuildingAddressablesLabel, so => {}).Completed +=
				OnMilitaryInfoReceived;
		}

		private void OnCivilianInfoReceived(AsyncOperationHandle<IList<IBuildingsSO>> asyncOperation)
		{
			if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
			{
				List<IBuildingsSO> civilianBuildingsInfo = asyncOperation.Result.ToList();
				OnCivilianBuildingsLoaded?.Invoke(this, civilianBuildingsInfo);
				CustomDebugger.Log(LogCategories.Addressables, "Civilian Buildings Info received");
			}
			else
			{
				CustomDebugger.LogError(LogCategories.Addressables, "Failed to load CivilianBuilding Info from addressables");
			}
		}
		
		private void OnMilitaryInfoReceived(AsyncOperationHandle<IList<IBuildingsSO>> asyncOperation)
		{
			if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
			{
				List<IBuildingsSO> civilianBuildingsInfo = asyncOperation.Result.ToList();
				OnMilitaryBuildingsLoaded?.Invoke(this, civilianBuildingsInfo);
				CustomDebugger.Log(LogCategories.Addressables, "Military Buildings Info received");
			}
			else
			{
				CustomDebugger.LogError(LogCategories.Addressables, "Failed to load MilitaryBuilding Info from addressables");
			}
		}
	}

}


