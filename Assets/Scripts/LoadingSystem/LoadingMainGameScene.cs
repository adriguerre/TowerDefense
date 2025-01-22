using System;
using System.Collections;
using DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadingMainGame
{
    public class LoadingMainGameScene : Singleton<LoadingMainGameScene>
    {
        
        #region Public Fields 
        public static LoadingMainGameScene Instance;
        #endregion 	

        #region Private Fields
        [Header("Custom loading wait")]
        [SerializeField] private bool bIsCustomLoadingWait;
        [SerializeField] private float loadingWaitOffset;
        [SerializeField] private Animator loadingAnimator;

        #endregion 	
        

        #region Unity Methods
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                Debug.LogWarning($"[GameManager.cs] There should never be more than one GameManager");
            }
            Instance = this;
        }

        void Start()
        {
            //TODO KW: Check if habia partida o no, por ahora del tiron a campaña
            //Hay que hacer un sistema para poner una pestaña de guardado, cuando llamamos al test level, se llama a este start de nuevo, y se crea otra campaign scene
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
        #endregion
        
        #region Events Methods 
        public void OnGameLoaded()
        {
            StartCoroutine(OnGameLoadedCorroutine());
        }
        
        public IEnumerator OnGameLoadedCorroutine()
        {
            Debug.Log("Game Loaded, removing loading scene");
            if (bIsCustomLoadingWait)
            {
                yield return new WaitForSeconds(loadingWaitOffset);
            }
            loadingAnimator.SetTrigger("FinishLoad");
            yield return new WaitForSeconds(0.7f);
            CloseLoadingScene();
            yield return null;
        }
         
        #endregion

        #region Public Methods

        public void CloseLoadingScene()
        {
            //GameManager.Instance.ActivateComponentsAfterLoading();
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        }

        #endregion
    }
}
