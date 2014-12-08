using UnityEngine;
using System.Collections;

public class CameraPanning : MonoBehaviour
{
	bool panning = false;
	Vector3 prevMousePos;
	
	float zoomMaxSpeed = 20.0f;
	float zoomSmoothingFactor = 10.0f;
	float minZoom = 6.0f;
	float maxZoom = 12.0f;
	float zoomVelocity = 0;
	
	Rect cameraBounds = new Rect(-10, 10, 10, 10);

	void Awake ()
	{
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{	
		panning = true;
	}
	
	void OnUpAction (Vector3 position)
	{
		panning = false;
	}

	void Update()
	{
		if (panning == true)
		{
			Vector3 deltaMousePos = Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 newCameraPosition = this.transform.position + deltaMousePos;
			this.transform.position = newCameraPosition;
		}
	
		prevMousePos = Input.mousePosition;
		
		if (Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") < 0)
			zoomVelocity = Mathf.Lerp(zoomVelocity, zoomMaxSpeed, zoomSmoothingFactor * Time.deltaTime);
		else if (Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") > 0)
			zoomVelocity = Mathf.Lerp(zoomVelocity, -zoomMaxSpeed, zoomSmoothingFactor * Time.deltaTime);
		else
			zoomVelocity = Mathf.Lerp(zoomVelocity, 0, zoomSmoothingFactor * Time.deltaTime);
		
		camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + Time.deltaTime * zoomVelocity, minZoom, maxZoom);
		
		//bounds check and Lerp into correct position in bounds
		
	//	if (!cameraBounds.Contains(transform.position + camera.rect
	}
}
