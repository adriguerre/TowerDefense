using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This script is used to zoom In or out when selecting a level in the campaign
/// </summary>
public class CameraMovements : ISingleton<CameraMovements>
{
	//public static CameraMovements Instance;
	
	private Camera _mainCamera;
	
	private Vector2 pastPosition;
	private Vector2 levelPosition;

	[SerializeField] private float FocusZoomValue = 3;
	[SerializeField] private float UnfocusZoomValue = 5; 
	[SerializeField] private float AnimationTimeMultiplier = 10f;
	private bool needToFocusIn;
	private bool needToFocusOut;
	public bool canMoveCamera { get; private set; }
	private void Awake()
	{
		// if (Instance != null)
		// {
		// 	Destroy(this);
		// 	Debug.LogError("There is already an instance of the CameraMovements!");
		// }
		// Instance = this;
		base.Awake();
		canMoveCamera = true;
		_mainCamera = GetComponent<Camera>();
		pastPosition = Vector2.zero;
	}
	
	private void Update()
	{
		if (needToFocusIn)
		{
			CameraFocusIn();
		}else if (needToFocusOut)
		{
			CameraFocusOut();
		}
	}

	private void CameraFocusOut()
	{
		float currentValue = this.GetComponent<Camera>().orthographicSize;
		_mainCamera.orthographicSize = Mathf.Lerp(currentValue, UnfocusZoomValue, AnimationTimeMultiplier * Time.deltaTime);
		Vector3 cameraPosition = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, -10);
		_mainCamera.transform.position = Vector3.Lerp(cameraPosition, pastPosition, AnimationTimeMultiplier * Time.deltaTime);
		if (Vector2.Distance(_mainCamera.transform.position, pastPosition) <= 0.02f)
		{
			_mainCamera.orthographicSize = UnfocusZoomValue;
			_mainCamera.transform.position =  new Vector3(pastPosition.x, pastPosition.y, -10);;
			needToFocusOut = false;
			canMoveCamera = true;
		}
	}

	private void CameraFocusIn()
	{
		float currentValue = this.GetComponent<Camera>().orthographicSize;
		_mainCamera.orthographicSize = Mathf.Lerp(currentValue, FocusZoomValue, AnimationTimeMultiplier * Time.deltaTime);
		Vector3 cameraPosition = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, -10);
		_mainCamera.transform.position = Vector3.Lerp(cameraPosition, levelPosition, AnimationTimeMultiplier * Time.deltaTime);
		if (Vector2.Distance(_mainCamera.transform.position, levelPosition) <= 0.02f)
		{
			_mainCamera.orthographicSize = FocusZoomValue;
			_mainCamera.transform.position = new Vector3(levelPosition.x, levelPosition.y, -10);
			needToFocusIn = false;
		}
	}


	public void MoveCameraToFocusLevel(Vector2 levelPosition)
	{
		this.levelPosition = levelPosition;
		pastPosition = _mainCamera.transform.position;
		needToFocusIn = true;
		needToFocusOut = false;
		canMoveCamera = false;
	}


	public void ResetCameraPosition()
	{
		needToFocusIn = false;
		needToFocusOut = true;
	}
}