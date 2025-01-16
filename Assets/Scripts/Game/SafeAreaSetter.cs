using UnityEngine;

public class SafeAreaSetter : MonoBehaviour
{

    #region Public Fields 
    
    public Canvas canvas;
    
    #endregion 	

    #region Private Fields
    private RectTransform _panelSafeArea;
    private Rect _currentSafeArea = new Rect();
    ScreenOrientation _currentOrientation = ScreenOrientation.AutoRotation;
    #endregion 	

    #region Properties 
    #endregion 
	
    #region Events 
    #endregion 	

    #region Unity Methods

    void Awake()
    {
        
    }

    void Start()
    {
        _panelSafeArea = GetComponent<RectTransform>();
        _currentOrientation = Screen.orientation;
        _currentSafeArea = Screen.safeArea;
        ApplySafeArea();
    }

    void Update()
    {
        if (_currentOrientation != Screen.orientation || _currentSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }

    #endregion

    #region Public Methods 
    #endregion 	

    #region Private Methods

    private void ApplySafeArea()
    {
        if (_panelSafeArea == null)
            return;
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        Rect pixelRect = canvas.pixelRect;
        anchorMin.x /= pixelRect.width;
        anchorMin.y /= pixelRect.height;

        anchorMax.x /= pixelRect.width;
        anchorMax.y /= pixelRect.height;

        _panelSafeArea.anchorMin = anchorMin;
        _panelSafeArea.anchorMax = anchorMax;

        _currentOrientation = Screen.orientation;
        _currentSafeArea = Screen.safeArea;



    }
    #endregion 

    #region Getter & Setters 
    #endregion 	
	


}