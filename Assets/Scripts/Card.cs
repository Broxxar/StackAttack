using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	public float scale = 1.5f;
	public float moveSpeed = 20f;
	public float scaleUpSpeed = 6f;
	public float scaleDownSpeed = 12f;
	public Vector3 startingSize;
	public Component[] childColliders;
	public Component[] childRenderers;
	bool drag = false;
	bool oldDrag = false;
	private SpriteRenderer sprite;
	Card[] cards;

	void Start () {
		startingSize = transform.localScale;
		sprite = GetComponent<SpriteRenderer>();
		childColliders = GetComponentsInChildren<BoxCollider2D> ();
		childRenderers = GetComponentsInChildren<SpriteRenderer>();
		cards = FindObjectsOfType (typeof(Card)) as Card[];
	}
	void Awake (){
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
	}
	
	// Update is called once per frame
	void Update () {
		if (drag == true && oldDrag != drag) {
			sprite.sortingLayerID = 1;
			foreach(SpriteRenderer sprites in childRenderers){
				sprites.sortingLayerID = 1;
			}
		}
		else if(drag==false && oldDrag != drag) {
			sprite.sortingLayerID = 0;
			foreach(SpriteRenderer sprites in childRenderers){
				sprites.sortingLayerID = 0;
			}
		}

		if (drag == true) {	
			SnapToCenter (); 
			SmoothScaleUp();
		}
		else if(!drag && OnGame()) {
			AddToGame();
		}
		else if(!drag && !OnGame()){
			SmoothScaleDown();
			OverlappingOnDesk();
		}
		oldDrag = drag;
	}

	void SnapToCenter() {
		Vector3 newPosition = Vector3.Lerp(this.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime*moveSpeed);
		transform.position = new Vector3 (newPosition.x, newPosition.y, 0);
	}
	
	void SmoothScaleUp() {
		transform.localScale = Vector3.Lerp(transform.localScale,startingSize*scale, Time.deltaTime*scaleUpSpeed);
	}
	void SmoothScaleDown() {
		transform.localScale = Vector3.Lerp(transform.localScale,startingSize, Time.deltaTime*scaleDownSpeed);
	}

	bool OnGame(){
		return false;
	}
	void AddToGame (){
		foreach(BoxCollider2D colliders in childColliders){
			colliders.isTrigger = true;
		}
	}

	void OverlappingOnDesk (){
		foreach (Card card in cards) {
			if (renderer.bounds.Intersects(card.renderer.bounds) && card.drag==false) {
				Vector3 newVector =  transform.position - card.transform.position;
				transform.position += newVector * Time.deltaTime;
			}
		}
	}

	void OnDownAction (Vector3 position){
		drag = true;
	}
	void OnUpAction (Vector3 position){
		drag = false;
	}
}
