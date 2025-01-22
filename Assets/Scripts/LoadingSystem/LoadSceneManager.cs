using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    
    public static LoadSceneManager Instance;
    
    [Header("Custom loading wait")]
    [SerializeField] private bool bIsCustomLoadingWait;
    [SerializeField] private float loadingWaitOffset;
    [SerializeField] private Animator loadingAnimator;
    [SerializeField] private GameObject canvasGameObject;
    [SerializeField] private GameObject cameraGameObject;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is already a LoadSceneManager attached to a single Scene!");
            Destroy(this);
        }

        Instance = this;
    }

    public void StartLoadAnimation()
    {
        cameraGameObject.SetActive(true);
        canvasGameObject.SetActive(true);
    }
    public void FinishLoadAnimation()
    {
        cameraGameObject.SetActive(false);
        canvasGameObject.SetActive(false);
    }
    public void OpenLevel()
    {      
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        StartLoadAnimation();
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
    }
    
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

    private void CloseLoadingScene()
    {
        FinishLoadAnimation();
    }

}