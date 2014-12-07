using UnityEngine;
using System.Collections;

public class CameraPanning : MonoBehaviour {
	bool panning = false;
	Vector3 prevMousePos;

	// limits for panning

	float zoomSpeed = 8.0f;
	float minSize = 6.0f;
	float maxSize = 16.0f;
	float startSize = 8.0f;
	float startMinX = -8;
	float startMinY = -8;
	float startMaxX = 8;
	float startMaxY = 8;	
	float minX;
	float minY;
	float maxX;
	float maxY;

	void Awake ()
	{
	
		camera.orthographicSize = startSize;
		minX = startMinX;
		minY = startMinY;
		maxX = startMaxX;
		maxY = startMaxY;
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

	public void LoadScene(string sceneName)
	{
		if (sceneName == "")
		{
			sceneName = Application.loadedLevelName;
		}
		Application.LoadLevel (sceneName);
	}


	void Update(){
		if (panning == true) {

			Vector3 deltaMousePos = Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Vector3 mousePos = Camera.main.ScreenToWorldPoint(deltaMousePos);

			Vector3 newCameraPosition = this.transform.position + deltaMousePos; //new Vector3 (mousePos.x, mousePos.y, this.transform.position.z); 
			if (newCameraPosition.x > maxX)
			{
				newCameraPosition.x = maxX;
			}
			else if (newCameraPosition.x < minX)
			{
				newCameraPosition.x = minX;
			} 
			if (newCameraPosition.y > maxY)
			{
				newCameraPosition.y = maxY;
			}
			else if (newCameraPosition.y < minY)
			{
				newCameraPosition.y = minY;
			}

			this.transform.position = newCameraPosition;
		}
		prevMousePos = Input.mousePosition;
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Plus)|| Input.GetKey(KeyCode.Equals)|| Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (camera.orthographicSize > minSize)
			{
				camera.orthographicSize -= Time.deltaTime * zoomSpeed;
				minX = startMinX - startSize  + camera.orthographicSize;
				maxX = startMaxX + startSize - camera.orthographicSize;
				minY = startMinY - startSize  + camera.orthographicSize;
				maxY = startMaxY + startSize - camera.orthographicSize;
			}
		}
		else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Minus) || Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (camera.orthographicSize < maxSize)
			{
			    camera.orthographicSize += Time.deltaTime * zoomSpeed;
				minX = startMinX - startSize  + camera.orthographicSize;
				maxX = startMaxX + startSize - camera.orthographicSize;
				minY = startMinY - startSize  + camera.orthographicSize;
				maxY = startMaxY + startSize - camera.orthographicSize;
			}
		}

//		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
//		{
//			camera.orthographicSize = Mathf.Min(Camera.main.orthographicSize-1, 6);
//		}

	}
}
