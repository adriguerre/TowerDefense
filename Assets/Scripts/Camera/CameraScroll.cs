using UnityEngine;

public class CameraScroll : Singleton<CameraScroll>
{
    [SerializeField] private Camera cam;

    Vector3 touchStart;
    private float topLimit = 25f;
    private float bottomLimit = 4.67f;

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
    
    void Update()
    {
        PanCamera();

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

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Button Down");
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
                Debug.Log("Scrolling...");

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
                Debug.Log("Click detected");
                
            }

            isMovingCamera = false;
        }
    }
}
