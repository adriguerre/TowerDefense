using System.Collections;
using UnityEngine;

public class DelayStaticAnimation : MonoBehaviour
{

    #region Public Fields 
    #endregion 	

    #region Private Fields

    [SerializeField] private float minTimeToStart = 0;
    [SerializeField] private float maxTimeToStart = 0.5f;
    
    private Animator _animator;
    #endregion 	
    

    #region Unity Methods
    

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartAnimation();
    }
    
    #endregion
    
    private void StartAnimation()
    {
        float getTimeToStart = UnityEngine.Random.Range(minTimeToStart, maxTimeToStart);
        StartCoroutine(StartInTime(getTimeToStart));
    }
    private IEnumerator StartInTime(float getTimeToStart)
    {
        yield return new WaitForSeconds(getTimeToStart);
        if(_animator != null)
            _animator.SetTrigger("StartStaticAnimation");
    }
}