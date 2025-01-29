using UnityEngine;

public abstract class ISingleton<T> : MonoBehaviour where T : ISingleton<T>
{
    private static T instance;
    [Space(10)]
    [Header("Singleton")]
    [HideInInspector] [SerializeField] protected bool _isPersistentAcrossScenes = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.Log((typeof(T) +"There is no instance." ));
                    //var newInstance = new GameObject(typeof(T).Name).AddComponent<T>();
                    //instance = newInstance;
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;

        if (instance == null)
        {
            instance = this as T;
            if (_isPersistentAcrossScenes)
            {
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


}