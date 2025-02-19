using System;
using System.Collections;
using Buildings.MilitaryBuildings;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    
    public static CameraScroll Instance;
    
    [SerializeField] private Camera cam;

    Vector3 touchStart;
    [field: SerializeField] private float topLimit = 25f;
    [field: SerializeField] private float bottomLimit = 4.67f;

    private Vector3 _curPosition;
    private Vector3 _velocity;
    private bool _underInertia;
    private float _time = 0.0f;
    [HideInInspector] public float SmoothTime = 2;
    [HideInInspector] public Vector3 direction;
    [SerializeField] float clickDurationThreshold = 0.2f; // Time to detect is a click
    private float clickTimer = 0.0f;
    private bool isClick = false;
    //If canMoveCamera is false, players won't be able to scroll
    [field: SerializeField] private bool canMoveCamera { get; set; }
    
    //Variable used when we need to center camera in any buildings
    private bool IsBeingCentered { get; set; }
    [field: SerializeField] float TimeToGetCentered { get; set; }
    private Vector3 moveToPosition;
    private Coroutine enableCameraMovementCoroutine;
    public Action onCameraCenterCompleted;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            Debug.LogError("There is already a camera scroll attached to this GameObject");
        }
        Instance = this;
        SetIfPlayerCanMoveCamera(true);

    }

    void Update()
    {
        if (IsBeingCentered)
        {
            CenterCameraOnTarget();
        }
        if (!canMoveCamera)
        {
            return;
        }
        
        PanCamera();
        CameraMovementInScroll();
    }

    private void CenterCameraOnTarget()
    {
        if (moveToPosition.y < bottomLimit)
        {  
            IsBeingCentered = false;
            SetIfPlayerCanMoveCamera(true);

            onCameraCenterCompleted?.Invoke();
            onCameraCenterCompleted = null;
            
            return;
        }
        SetIfPlayerCanMoveCamera(false);
        Vector3 cameraPosition = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
        cam.transform.position = Vector3.Lerp(cameraPosition, moveToPosition, TimeToGetCentered * Time.deltaTime);
        if (Vector2.Distance(cam.transform.position, moveToPosition) <= 0.02f)
        {
            cam.transform.position =
                new Vector3(cam.transform.position.x, moveToPosition.y, cam.transform.position.z);
            IsBeingCentered = false;
            SetIfPlayerCanMoveCamera(true);

            onCameraCenterCompleted?.Invoke();
            onCameraCenterCompleted = null;
        }
    }



    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Button Down");
            touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
            _underInertia = false;
            clickTimer = 0.0f; // Inicia el temporizador
            isClick = true; // Asume que es un clic hasta que se demuestre lo contrario
        }

        if (Input.GetMouseButton(0))
        {
            clickTimer += Time.deltaTime;

            // Si se sobrepasa el umbral, considera que no es un clic
            if (clickTimer > clickDurationThreshold)
            {
                if (!CivilianBuildingsUIManager.Instance.playerIsTryingToStartConstruction || !MilitaryBuildingsUIManager.Instance.playerIsTryingToStartConstruction)
                {
                    LevelGrid.Instance.DesactivateGridSlotPrefabAndHideBuildUIPop();
                }

                isClick = false;
                //isMovingCamera = true;
                //Debug.Log("Scrolling...");

                direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
                float finalYPos = cam.transform.position.y + direction.y;
                finalYPos = Mathf.Clamp(finalYPos, bottomLimit, topLimit);
                Vector3 desiredPosition = new Vector3(cam.transform.position.x, finalYPos, cam.transform.position.z);
                cam.transform.position = desiredPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Si al soltar el botón sigue siendo un clic, ejecuta la acción de clic
            if (isClick)
            {
                if (!CivilianBuildingsUIManager.Instance
                        .playerIsTryingToStartConstruction && !MilitaryBuildingsUIManager.Instance
                        .playerIsTryingToStartConstruction) //Selecting all posible grids slots
                {
                    if (LevelGrid.Instance != null)
                    {
                        LevelGrid.Instance.ClickOnLevelGrid();
                    }
                }
                else if (CivilianBuildingsUIManager.Instance
                         .playerIsTryingToStartConstruction) //Selecting civilian building position
                {
                    if (LevelGrid.Instance != null)
                    {
                        LevelGrid.Instance.SelectSlotAsPossibleCivilianLocation();
                    }
                }
                else if (MilitaryBuildingsUIManager.Instance
                         .playerIsTryingToStartConstruction) // Selecting military building position
                {
                    if (LevelGrid.Instance != null)
                    {
                        LevelGrid.Instance.SelectSlotAsPossibleMilitaryLocation();
                    }
                }
            }
            //isMovingCamera = false;
        }
    }

    public void CenterCameraOnBuildingWithCallback(float positionY, Action onComplete)
    {
        moveToPosition = new Vector3(cam.transform.position.x, positionY, cam.transform.position.z);
        IsBeingCentered = true;
        onCameraCenterCompleted = onComplete;
    }
    public void CenterCameraOnBuilding(float positionY)
    {
        moveToPosition = new Vector3(cam.transform.position.x, positionY, cam.transform.position.z);
        onCameraCenterCompleted = null;
        IsBeingCentered = true;
    }

    private void CameraMovementInScroll()
    {
        if (_underInertia && _time <= SmoothTime)
        {
            cam.transform.position += _velocity;
            float newY = Mathf.Clamp(cam.transform.position.y, bottomLimit, topLimit);
            cam.transform.position = new Vector3(cam.transform.position.x, newY, cam.transform.position.z);

            _velocity = Vector3.Lerp(_velocity, Vector3.zero, _time);
            _time += Time.smoothDeltaTime;
        }
        else
        {
            _underInertia = false;
            _time = 0.0f;
        }

        // Resetea la lógica de clic si no se mantiene el botón
        if (!Input.GetMouseButton(0))
        {
            clickTimer = 0.0f;
            isClick = false;
        }
    }

    public void SetIfPlayerCanMoveCamera(bool value)
    {
        if (value)
        {
            //canMoveCamera = true;
            enableCameraMovementCoroutine = StartCoroutine(SetCameraMoveValueToTrue()); 
        }
        else
        {
            if (enableCameraMovementCoroutine != null)
            {
                StopCoroutine(enableCameraMovementCoroutine);
                enableCameraMovementCoroutine = null;
            }
            canMoveCamera = false;
        }
            
    }

    private IEnumerator SetCameraMoveValueToTrue()
    {
       yield return new WaitForSeconds(0.1f);
       canMoveCamera = true;
    }
}
