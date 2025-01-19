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
    private void LoadGameData()
    {
        //Load game data
        //Remove loading screen
        if (LoadingMainGameScene.Instance != null)
            LoadingMainGameScene.Instance.OnGameLoaded();
        else //KW Testing purposes
            ActivateComponentsAfterLoading();
        //Activate audio listener to main camera
        //SaveManager.Instance.onPlayerDataLoaded += OnPlayerDataLoaded;
        //SaveManager.Instance.LoadPlayerData();
    }

    // private void OnPlayerDataLoaded(object sender, PlayerData.PlayerData e)
    // {
    //     _playerData = e;
    //     Debug.Log("PLAYER DATA LOADED: KW " + _playerData.choppersUnlocked);
    //     //Spawn choppers
    // }
    
    public void ActivateComponentsAfterLoading()
    {
        _mainCamera.SetActive(true);
        mainNavBarUI.SetActive(true);
        topBarUI.SetActive(true);
    }
    
}