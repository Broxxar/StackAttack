using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	CharController2D player;
	Collider2D[] stuff;
	public string goal;
	public int cheeseGoal = 0;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType (typeof(CharController2D)) as CharController2D;
		stuff = player.GetComponentsInChildren<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && 0 == cheeseGoal) {	
			foreach(Collider2D thing in stuff){
				if(renderer.bounds.Intersects(thing.bounds)){
					Application.LoadLevel (goal);
					break;
				}
			}
		}
	}
}
