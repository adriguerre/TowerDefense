using System.Collections.Generic;
using LoadingMainGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{
	[SerializeField] private List<Button> _levelsList;

	[SerializeField] private Button _testLevelButton;


    void Awake()
    {
        
    }

    void Start()
    {
	    LoadGameData();
        _testLevelButton.onClick.AddListener(() => OpenTestLevel());
    }

    private void OpenTestLevel()
    {
	    SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
		LoadingMainGameScene.Instance.OpenTestLevel();
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