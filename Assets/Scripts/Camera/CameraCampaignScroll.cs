using UnityEngine;

public class CameraCampaignScroll : MonoBehaviour
{

       [SerializeField] private Camera cam;



    private Vector3 _curPosition;
    private Vector3 _velocity;
    private bool _underInertia;
    private float _time = 0.0f;
    public float SmoothTime = 2;
    public bool isMovingCamera = false;
    public Vector3 direction;
    [SerializeField] float clickDurationThreshold = 0.2f; // Time to detect is a click
    private float clickTimer = 0.0f;
    private bool isClick = false;
    Vector3 touchStart;
    private float topLimit = 3.3f;
    private float bottomLimit = 1.09f;
    private float leftLimit = -3.25f; // Agrega un límite izquierdo
    private float rightLimit = 11.19f; // Agrega un límite derecho
    
    
    void Update()
    {
        if (!CameraMovements.Instance.canMoveCamera)
        {
            return;
        }
        PanCamera();

        if (_underInertia && _time <= SmoothTime)
        {
            cam.transform.position += _velocity;

            float newY = Mathf.Clamp(cam.transform.position.y, bottomLimit, topLimit);
            float newX = Mathf.Clamp(cam.transform.position.x, leftLimit, rightLimit);
            cam.transform.position = new Vector3(newX, newY, cam.transform.position.z);

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
                isClick = false;
                isMovingCamera = true;
                //Debug.Log("Scrolling...");

                direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
                float finalYPos = cam.transform.position.y + direction.y;
                float finalXPos = cam.transform.position.x + direction.x;

                finalYPos = Mathf.Clamp(finalYPos, bottomLimit, topLimit);
                finalXPos = Mathf.Clamp(finalXPos, leftLimit, rightLimit);

                Vector3 desiredPosition = new Vector3(finalXPos, finalYPos, cam.transform.position.z);
                cam.transform.position = desiredPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Si al soltar el botón sigue siendo un clic, ejecuta la acción de clic
            if (isClick)
            {
                Debug.Log("Click detected");
            }

            isMovingCamera = false;
        }
    }


}