using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
	public float moveSpeed = 20f;
	public float scaleUpSpeed = 6f;
	public float scaleDownSpeed = 12f;
	public Vector3 startingSize;
	public Component[] childColliders;
	public Component[] childRenderers;
	
	float pickupScale = 1.2f;
	bool drag = false;
	bool oldDrag = false;
	private Collider2D collide;
	private CardManager cm;
	public bool inStack = false;
	 bool rotateFocus = false;
	 Vector3 targetRotation;
	 float zTimes = 0;
	Card[] cards;

	void Awake (){
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
		
		GetComponent<Clickable>().RightUpAction += OnRightUpAction;
	}
	
	void Start () {
		startingSize = transform.localScale;
		collide = GetComponent<Collider2D> ();
		childColliders = GetComponentsInChildren<Collider2D> ();
		childRenderers = GetComponentsInChildren<SpriteRenderer>();

		cm = FindObjectOfType (typeof(CardManager)) as CardManager;
		cards = FindObjectsOfType (typeof(Card)) as Card[];
		DisableAllColliders ();
		collide.enabled = true;

		zTimes = (transform.localEulerAngles.z / 180f) + 1;
	}

	void Update () {
		if (rotateFocus) {
			Rotate();
		}

		if (drag == true && oldDrag != drag) {
			collide.enabled = false;
			foreach(SpriteRenderer sprites in childRenderers){
				sprites.sortingLayerID = 1;
			}
		}
		else if(drag==false && oldDrag != drag) {
			collide.enabled = true;
			if(OnGame()){
				foreach(SpriteRenderer sprites in childRenderers){
					sprites.sortingLayerID = 3;
				}
			}
			else{
				foreach(SpriteRenderer sprites in childRenderers){
					sprites.sortingLayerID = 0;
				}
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
				foreach(SpriteRenderer sprites in childRenderers){
					sprites.sortingLayerID = 0;
				}
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
		transform.localScale = Vector3.Lerp(transform.localScale,startingSize*pickupScale, Time.deltaTime*scaleUpSpeed);
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
		foreach (Card card in cards) {
			if (collide.bounds.Intersects(card.collide.bounds) && card.drag==false) {
				Vector3 newVector =  transform.position - card.transform.position;
				transform.position += newVector * Time.deltaTime;
			}
		}
		if (collide.bounds.Intersects(cm.renderer.bounds)) {
			Vector3 newVector =  transform.position - cm.transform.position;
			transform.position += newVector * Time.deltaTime;
		}
	}

	public void EnableAllColliders(){
		foreach(Collider2D colliders in childColliders){
			if(colliders.gameObject.CompareTag("Ladder")){
				Vector3 ladderRotate = new Vector3(0f,0f,(colliders.gameObject.transform.rotation.z/180f)%2f);
				colliders.gameObject.transform.rotation = Quaternion.Euler(ladderRotate);
			}
			colliders.enabled = true;
		}
		collide.enabled = false;
	}

	public void DisableAllColliders(){
		foreach(Collider2D colliders in childColliders){
			colliders.enabled = false;
		}
		collide.enabled = true;
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
		if(Mathf.Abs(targetRotation.z - transform.localEulerAngles.z) < 2f)
		{
			rotateFocus=false;
			transform.rotation = Quaternion.Euler(targetRotation);
		}
		else
		{
			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, 10f * Time.deltaTime); 
		}
	}

	public void Focus 	()					{ drag = true;	}
	void OnDownAction 	(Vector3 position)	{ drag = true;	}
	void OnUpAction (Vector3 position) { drag = false;	}
	
	void OnRightUpAction 	(Vector3 position)
	{ 
		rotateFocus = true; 
		targetRotation = (180f*(zTimes%2)) * Vector3.forward;	
		zTimes++;
	}
}
