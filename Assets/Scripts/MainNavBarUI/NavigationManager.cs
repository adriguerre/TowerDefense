using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainNavBarUI
{
	
	public class NavigationManager : Singleton<NavigationManager>
	{

		#region Public Fields 
		#endregion 	

		#region Private Fields
		
		[FormerlySerializedAs("warbondsButton")]
		[Header("Nav Buttons")]
		[SerializeField] private Button button1;
		[SerializeField] private Button button2;
		[SerializeField] private Button gameplayButton;
		[SerializeField] private Button button4;
		[SerializeField] private Button button5;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button profileButton;
		
		[Header("Pressed Button")]
		[SerializeField] private Sprite pressedButtonSprite;
		private Sprite defaultButtonSprite;
		
		private TabTypes currentTabOpened = TabTypes.None;
		private Button currentButtonClicked;
		#endregion 	

		#region Properties

		public Button WarbondButton => button1;
		public Button Button2 => button2;
		public Button GameplayButton => gameplayButton;
		public Button Button4 => button4;
		public Button Button5 => button5;
		
		#endregion 
	
		#region Events 
		#endregion 	

		#region Unity Methods

		void Awake()
		{
        
		}

		void Start()
		{
			button1.onClick.AddListener(() => OpenScreenCanvas(TabTypes.Button1));
			button2.onClick.AddListener(() => OpenScreenCanvas(TabTypes.CivilianBuildings));
			gameplayButton.onClick.AddListener(() => OpenScreenCanvas(TabTypes.Gameplay));
			button4.onClick.AddListener( () => OpenScreenCanvas(TabTypes.Button4));
			button5.onClick.AddListener(    () => OpenScreenCanvas(TabTypes.Button5));
			settingsButton.onClick.AddListener(    () => OpenScreenCanvas(TabTypes.Settings));
			//profileButton.onClick.AddListener( () => OpenScreenCanvas(TabTypes.Profile));


			OpenScreenCanvas(TabTypes.Gameplay);
		}

		private void Update()
		{
			//if (Input.GetKeyDown(KeyCode.Escape))
			//{
			//	CloseCurrentTab();
			//}
		}

		#endregion
		

		#region Public Methods 
		
		
		/// <summary>
		/// Close current screen canvas
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public void CloseCurrentTab()
		{
			switch (currentTabOpened)
			{
				case TabTypes.Button1:
					ResearchUIManager.Instance.CloseResearchUI();
					break;		
				case TabTypes.CivilianBuildings:
					CivilianBuildingUIPanel.Instance.CloseCivilianBuildingUI();
					break;		
				case TabTypes.Gameplay:
					break;		
				case TabTypes.Button4:
					MilitaryUIManager.Instance.CloseMilitaryUI();
					break;		
				case TabTypes.Button5:
					MarketUIManager.Instance.CloseMarketUI();
					break;
				case TabTypes.Settings:
					SettingsUIManager.Instance.CloseSettingsUI();
					break;
				case TabTypes.Profile:
					//ProfileUIManager.Instance.CloseSettingsUI();
					break;
			}
			
			CameraScroll.Instance.canMoveCamera = true;
			if(currentTabOpened != TabTypes.Settings)
				currentButtonClicked.GetComponent<Image>().sprite = defaultButtonSprite;
			currentTabOpened = TabTypes.None;
		}
		#endregion 	

		#region Private Methods
		
		public void OpenScreenCanvas(TabTypes type)
		{
			if (type != TabTypes.Settings && type == currentTabOpened)
				return;
			
			if(currentTabOpened != TabTypes.None)
				CloseCurrentTab();

			CameraScroll.Instance.canMoveCamera = false;
			currentTabOpened = type;
			
			switch (type)
			{
				case TabTypes.Button1:
					OpenWarbondScreen();
					currentButtonClicked = button1;
					break;		
				case TabTypes.CivilianBuildings:
					OpenMilitaryScreen();
					currentButtonClicked = button2;
					break;		
				case TabTypes.Gameplay:
					OpenGameplayScreen();
					currentButtonClicked = gameplayButton;
					break;		
				case TabTypes.Button4:
					OpenFactoryScreen();
					currentButtonClicked = button4;
					break;		
				case TabTypes.Button5:
					OpenModsScreen();
					currentButtonClicked = button5;
					break;				
				case TabTypes.Settings:
					OpenSettingsScreen();
					break;
				case TabTypes.Profile:
					OpenProfileScreen();
					break;
			}
		}

		
		private void OpenWarbondScreen()
		{
			ChangeButtonSprite(button1);
			ResearchUIManager.Instance.openResearchUI();
		}
		private void OpenMilitaryScreen()
		{
			ChangeButtonSprite(button2);
			CivilianBuildingUIPanel.Instance.OpenCivilianBuildingUI();
		}

		private void OpenGameplayScreen()
		{
			CameraScroll.Instance.canMoveCamera = true;
			ChangeButtonSprite(gameplayButton);
		}

		private void OpenFactoryScreen()
		{
			ChangeButtonSprite(button4);
			MilitaryUIManager.Instance.OpenMilitaryUI();
		}

		private void OpenModsScreen()
		{
			ChangeButtonSprite(button5);
			MarketUIManager.Instance.OpenMarketUI();
		}

		private void OpenSettingsScreen()
		{
			SettingsUIManager.Instance.SwapSettingsVisibility();
		}

		private void OpenProfileScreen()
		{
			//ProfileUIManager.Instance.OpenProfileUI();
		}
		
		private void ChangeButtonSprite(Button button)
		{
			Image image = button.GetComponent<Image>();

			defaultButtonSprite = image.sprite;
			image.sprite = pressedButtonSprite;
		}
		
		#endregion 

		#region Getter & Setters 
		#endregion 	
	


	}
}
