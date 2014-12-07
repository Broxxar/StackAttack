using UnityEngine;
using System.Collections;

public class CameraPanning : MonoBehaviour {
	bool panning = false;
	Vector3 prevMousePos;

	// limits for panning
	int minX = -4;
	int minY = -2;
	int maxX = 4;
	int maxY = 2;

	void Awake ()
	{
	
		
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{
		
		panning = false;
		
	}
	
	void OnUpAction (Vector3 position)
	{
		panning = false;
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
	}
}
