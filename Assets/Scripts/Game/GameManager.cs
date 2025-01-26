using System;
using LoadingMainGame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Public Fields 
    public GameObject GetResourcesPanel => topBarUI;
    #endregion 	

    #region Private Fields 
    [Header("Main Camera")]
    [field: SerializeField] private GameObject _mainCamera { get; set; }

    [SerializeField] private GameObject mainNavBarUI;
    [SerializeField] private GameObject topBarUI;
    public LevelSO levelInfo { get; private set; }
    //private PlayerData.PlayerData _playerData;
    #endregion 	

    #region Properties 
    #endregion 	
    
    #region Events 
    #endregion


    #region Unity Methods
    private void Awake()
    {
        _mainCamera.SetActive(false);
        mainNavBarUI.SetActive(false);
        topBarUI.SetActive(false);
    }
    
    void Start()
    {
        LoadGameData();
    }
    
    void Update()
    {
        
    }
    
    #endregion


    /// <summary>
    /// Load all game data needed, when finished, destroy loading main game panel
    /// </summary>
    private async void LoadGameData()
    {
        
        if (LoadSceneManager.Instance != null)
        {
            levelInfo = LoadSceneManager.Instance.levelInfoToLoad;
            await LevelGrid.Instance.CreateLevel(levelInfo);
            LoadSceneManager.Instance.OnGameLoaded();
            ActivateComponentsAfterLoading();
        }
        else
        {
            LevelGrid.Instance.CreateLevel(levelInfo); 
            ActivateComponentsAfterLoading();
        }
    }
    
    public void ActivateComponentsAfterLoading()
    {
        _mainCamera.SetActive(true);
        mainNavBarUI.SetActive(true);
        topBarUI.SetActive(true);
    }
    
}