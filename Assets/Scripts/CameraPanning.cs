using UnityEngine;
using System.Collections;

public class CameraPanning : MonoBehaviour {
	bool panning = false;
	Vector3 prevMousePos;
	void Awake ()
	{
	
		
		GetComponent<Clickable>().DownAction += OnDownAction;
		GetComponent<Clickable>().UpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{
		
		panning = true;
		
	}
	
	void OnUpAction (Vector3 position)
	{
		panning = false;
	}
	
	void Update(){
		if (panning == true) {

			Vector3 deltaMousePos = Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Vector3 mousePos = Camera.main.ScreenToWorldPoint(deltaMousePos);

			this.transform.position += deltaMousePos; //new Vector3 (mousePos.x, mousePos.y, this.transform.position.z); 
		}
		prevMousePos = Input.mousePosition;
	}
}
