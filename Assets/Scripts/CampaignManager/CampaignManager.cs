using System.Collections.Generic;
using LoadingMainGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignManager : ISingleton<CampaignManager>
{
	[Header("Buttons")]
	[SerializeField] private Button _testLevelButton;
	[SerializeField] private Button _playLevelButton;
	[Header("Details Panel")]
	[SerializeField] private GameObject levelInfoPanel;
	[SerializeField] private TextMeshProUGUI levelNameText;
	
	[Header("Prefab")]
	[SerializeField] private GameObject levelSelectorPrefab;
	
	
	GameObject currentLevelSelectedPrefab;
	int currentLevelSelected;
	LevelSO currentInfoLevelSelected;


    void Start()
    {
	    LoadGameData();
        _testLevelButton.onClick.AddListener(() => OpenTestLevel());
        _playLevelButton.onClick.AddListener(() => OpenLevelInfo());
        _playLevelButton.gameObject.SetActive(false);
    }

 

    public void OpenUILevel(Level level, LevelSO levelInfo)
    {
	    if (currentLevelSelected == level.level)
	    {
		    //if we pick the same level, we destroy the prefab
		    Destroy(currentLevelSelectedPrefab);
		    levelNameText.text = "Catalyst";
		    currentLevelSelected = 0;
		    currentInfoLevelSelected = null;
		    CameraMovements.Instance.ResetCameraPosition();
		    _playLevelButton.gameObject.SetActive(false);
		    return;
	    }
	   
	    CameraMovements.Instance.MoveCameraToFocusLevel(level.gameObject.transform.position);
	    Destroy(currentLevelSelectedPrefab);
	    currentLevelSelected = level.level;
	    currentInfoLevelSelected = levelInfo;
	    _playLevelButton.gameObject.SetActive(true);

	    currentLevelSelectedPrefab = Instantiate(levelSelectorPrefab, level.gameObject.transform.position, Quaternion.identity);
	    if(levelInfo != null)
			levelNameText.text = levelInfo.levelName;
	    else
		    levelNameText.text = "Catalyst";
	    
    }
    private void OpenLevelInfo()
    {
	    levelInfoPanel.GetComponent<LevelInfoDetails>().SetProperties(currentInfoLevelSelected);
	    levelInfoPanel.SetActive(true);
    }
    private void OpenTestLevel()
    {
	    //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		LoadSceneManager.Instance.OpenLevel();
    }

    public void OpenLevel(LevelSO levelInfo)
    {
	    //TODO KW: Make level load with the grid it is
	    LoadSceneManager.Instance.OpenLevel();
    }
    
    

    private void LoadGameData()
    {
	    //Load game data
	    //Remove loading screen
	    if (LoadingMainGameScene.Instance != null)
		    LoadingMainGameScene.Instance.OnGameLoaded();
	    //Activate audio listener to main camera
	    //SaveManager.Instance.onPlayerDataLoaded += OnPlayerDataLoaded;
	    //SaveManager.Instance.LoadPlayerData();
    }
    



}