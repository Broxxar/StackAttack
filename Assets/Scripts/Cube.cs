using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
	bool drag = false;

	void Awake ()
	{

		

		GetComponent<Clickable>().DownAction += OnDownAction;
		GetComponent<Clickable>().UpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{

		drag = true;

	}

	void OnUpAction (Vector3 position)
	{
		drag = false;
	}

	void Update(){
		if (drag == true) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.transform.position = new Vector3 (mousePos.x, mousePos.y, this.transform.position.z); 
		}
	}

}
