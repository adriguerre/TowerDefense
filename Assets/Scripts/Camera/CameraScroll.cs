using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] private Camera cam;

    Vector3 touchStart;
    private float topLimit = 25f;
    private float bottomLimit = 5.05f;

    private Vector3 _curPosition;
    private Vector3 _velocity;
    private bool _underInertia;
    private float _time = 0.0f;
    public float SmoothTime = 2;

    public Vector3 direction;
    
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
    }


    private void PanCamera()
    {

        if (Input.GetMouseButtonDown(0))
        {
            touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
            _underInertia = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 _prevPosition = _curPosition;

            direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
            float finalYPos = cam.transform.position.y + direction.y;
            finalYPos = Mathf.Clamp(finalYPos, bottomLimit, topLimit);
            Vector3 desiredPosition = new Vector3(cam.transform.position.x, finalYPos, cam.transform.position.z);
            cam.transform.position = desiredPosition;

        }
    }
}