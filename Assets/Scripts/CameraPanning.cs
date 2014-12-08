using UnityEngine;
using System.Collections;

public class CameraPanning : MonoBehaviour
{
	bool panning = false;
	Vector3 prevMousePos;
	
	float zoomMaxSpeed = 20.0f;
	float zoomSmoothingFactor = 10.0f;
	float minZoom = 5.5f;
	float maxZoom = 16.0f;
	float zoomVelocity = 0;
	
	Rect cameraBounds = new Rect(-30, 20, 60, -40);

	void Awake ()
	{
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
	}
	
	void OnDrawGizmos ()
	{
		Gizmos.color = Color.cyan;
	
		Gizmos.DrawLine(cameraBounds.min, cameraBounds.min + Vector2.right * cameraBounds.width);
		Gizmos.DrawLine(cameraBounds.min, cameraBounds.min + Vector2.up * cameraBounds.height);
		Gizmos.DrawLine(cameraBounds.max, cameraBounds.max - Vector2.right * cameraBounds.width);
		Gizmos.DrawLine(cameraBounds.max, cameraBounds.max - Vector2.up * cameraBounds.height);
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
		
		Vector3 cameraTopLeftCorner = new Vector3(transform.position.x - camera.orthographicSize * camera.aspect, transform.position.y + camera.orthographicSize);
		Vector3 cameraTopRightCorner = new Vector3(transform.position.x + camera.orthographicSize * camera.aspect, transform.position.y - camera.orthographicSize);
		
		Vector3 correction = Vector3.zero;
		
		if (cameraTopLeftCorner.x < cameraBounds.min.x)
			correction.x += cameraBounds.min.x - cameraTopLeftCorner.x;
		if (cameraTopLeftCorner.y > cameraBounds.min.y)
			correction.y += cameraBounds.min.y - cameraTopLeftCorner.y;
		
		if (cameraTopRightCorner.x > cameraBounds.max.x)
			correction.x += cameraBounds.max.x - cameraTopRightCorner.x;
		if (cameraTopRightCorner.y < cameraBounds.max.y)
			correction.y += cameraBounds.max.y - cameraTopRightCorner.y;
			
		transform.Translate(correction);
	}
}
