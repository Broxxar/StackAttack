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
	private Collider2D collide;
	private CardManager cm;
	public bool inStack = false;
	public bool rotateFocus = false;
	public Vector3 targetRotation;
	float zTimes=1;
	Card[] cards;

	void Start () {
		startingSize = transform.localScale;
		collide = GetComponent<Collider2D> ();
		childColliders = GetComponentsInChildren<Collider2D> ();
		childRenderers = GetComponentsInChildren<SpriteRenderer>();

		cm = FindObjectOfType (typeof(CardManager)) as CardManager;
		cards = FindObjectsOfType (typeof(Card)) as Card[];
		DisableAllColliders ();
		collide.enabled = true;
	}
	void Awake (){
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;

		GetComponent<Clickable>().RightUpAction += OnRightUpAction;
	}
	
	// Update is called once per frame
	void Update () {
		if (rotateFocus) {
			Rotate();
		}

		if (drag == true && oldDrag != drag) {
			foreach(SpriteRenderer sprites in childRenderers){
				sprites.sortingLayerID = 1;
			}
		}
		else if(drag==false && oldDrag != drag) {
			foreach(SpriteRenderer sprites in childRenderers){
				sprites.sortingLayerID = 0;
			}
		}

		if (drag == true) {	
			SnapToCenter (); 
			SmoothScaleUp();
		}
		else if(!drag && OnGame()) {
			if(ScaleToManager() && !inStack){ 
				inStack = true;
				cm.AddToStack(this); 
			}
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
		SpriteRenderer temp = cm.GetComponent<SpriteRenderer> ();
		float right = temp.bounds.max.x;
		float left = temp.bounds.min.x;
		float top =temp.bounds.max.y;
		float bottom = temp.bounds.min.y;
		if ((transform.position.x < right && transform.position.x > left) && (transform.position.y < top && transform.position.y > bottom)) {
			return true;
		}
		else{return false;}
	}

	void OverlappingOnDesk (){
		collide.enabled = true;
		foreach (Card card in cards) {
			if (collide.bounds.Intersects(card.collide.bounds) && card.drag==false) {
				Vector3 newVector =  transform.position - card.transform.position;
				transform.position += newVector * Time.deltaTime;
			}
		}
	}

	public void EnableAllColliders(){
		foreach(Collider2D colliders in childColliders){
			colliders.enabled = true;
		}
		collide.enabled = false;
	}

	public void DisableAllColliders(){
		foreach(Collider2D colliders in childColliders){
			colliders.enabled = false;
		}
		//collide.enabled = true;
	}

	bool ScaleToManager(){
		if((Vector3.Distance(transform.position,cm.transform.position)<0.1f) &&
		   (cm.transform.localScale.x - transform.localScale.x < 1f)){
			transform.position=cm.transform.position;
			transform.localScale = cm.transform.localScale;
			return true;
		}
		else{
			Vector3 newVector =  cm.transform.position - transform.position;
			transform.position = Vector3.MoveTowards(transform.position, newVector,10*Time.deltaTime);
			transform.localScale = Vector3.Lerp(transform.localScale,Vector3.one, Time.deltaTime*scaleUpSpeed);
			return false;
		}
	}

	public void AdjustSortOrder(int howMuch){
		foreach(SpriteRenderer sprites in childRenderers){
			sprites.sortingOrder += howMuch;
		}
	}

	void Rotate(){
		if(Mathf.Abs(targetRotation.z - transform.localEulerAngles.z) < 2f){
			rotateFocus=false;
			transform.rotation = Quaternion.Euler(targetRotation);

			for(int i = 0; i< childColliders.Length;i++){
				if(childColliders[i].tag == "LadderTop"){ childColliders[i].tag = "LadderBottom"; }
				else if(childColliders[i].tag == "LadderBottom"){ childColliders[i].tag = "LadderTop"; }
			}
		}
		else{
			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, 10f * Time.deltaTime); 
			//transform.rotation= Quaternion.Lerp(transform.rotation, new Quaternion(0f,0f,targetRotation,1), Time.deltaTime*0.1f);
		}
	}

	public void Focus 	()					{ drag = true;	}
	void OnDownAction 	(Vector3 position)	{ drag = true;	}
	void OnUpAction 	(Vector3 position)	{ drag = false;	}
	void OnRightUpAction 	(Vector3 position)	{ 
		rotateFocus = true; 
		targetRotation = (180f*(zTimes%2)) * Vector3.forward;	
		zTimes++;
	}
}
