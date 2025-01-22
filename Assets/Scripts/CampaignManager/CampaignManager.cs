using System.Collections.Generic;
using LoadingMainGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{
	
	public static CampaignManager Instance;
	
	[SerializeField] private Button _testLevelButton;
	[SerializeField] private GameObject levelSelectorPrefab;
	private GameObject currentLevelSelectedPrefab;
	private int currentLevelSelected;
    void Awake()
    {
	    if (Instance != null)
	    {
		    Destroy(this);
		    Debug.LogError("There is already a CampaignManager in the scene!");
	    }
	    Instance = this;
    }

    void Start()
    {
	    LoadGameData();
        _testLevelButton.onClick.AddListener(() => OpenTestLevel());
 
    }

    public void OpenUILevel(Level level, LevelSO levelInfo)
    {
	    if (currentLevelSelected == level.level)
	    {
		    //if we pick the same level, we destroy the prefab
		    Destroy(currentLevelSelectedPrefab);
		    currentLevelSelected = 0;
		    return;
	    }
	    Debug.Log(level.transform.position);
	    currentLevelSelectedPrefab = Instantiate(levelSelectorPrefab, level.transform.position, Quaternion.identity);
    }

    private void OpenTestLevel()
    {
	    //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
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