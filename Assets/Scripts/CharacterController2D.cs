using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	Vector2 walkSpeed = new Vector2 (3,0);
	Vector2 climbSpeed = new Vector2(0,3);
	public int numCheese = 0;
	bool isGrounded = false; //to know when mouse can walk
	bool canClimb = false;   
	Rigidbody2D body ;
	Animator anim;
	Transform mouseTrans;
	GameObject mouseVertical, mouseHorizontal, ladder;

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		mouseTrans = transform.FindChild("mouse");
		mouseHorizontal = mouseTrans.FindChild("mouse_body").gameObject;
		mouseVertical = mouseTrans.FindChild("mouse_body_vert").gameObject;
	}


	void FixedUpdate(){

		if (isGrounded) {//only want him to move sideways when on a platform
			mouseVertical.gameObject.SetActive( false);
			mouseHorizontal.gameObject.SetActive(true);
			if (Input.GetKey (KeyCode.A )|| Input.GetKey (KeyCode.LeftArrow)) {
				mouseTrans.localScale = new Vector3(-1,1,1);
				body.MovePosition (body.position - walkSpeed * Time.deltaTime);
				anim.SetBool("Running", true);
			} else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ){
				mouseTrans.localScale = new Vector3(1,1,1);
				body.MovePosition (body.position + walkSpeed * Time.deltaTime);
				anim.SetBool("Running", true);
			} else{
				anim.SetBool("Running", false);
			}
		}
		if(ladder != null ){//set to unrealistic number(8000) when mouse leaves trigger
			if(ladder.collider2D.enabled){
				gameObject.rigidbody2D.gravityScale = 0;

					if (Input.GetKey (KeyCode.S)| Input.GetKey(KeyCode.DownArrow)) {
					mouseTrans.localScale = new Vector3(1,-1,1);
					mouseVertical.SetActive( true);
					mouseHorizontal.SetActive(false);
					anim.SetBool("pushingButton", true);
					anim.SetBool("Climbing", true);
					canClimb = true;
					gameObject.layer = 9;
					body.MovePosition (new Vector2(ladder.transform.position.x,body.position.y) - climbSpeed * Time.deltaTime);

					} else if (Input.GetKey (KeyCode.W)| Input.GetKey(KeyCode.UpArrow)) {
					mouseTrans.localScale = new Vector3(1,1,1);
					mouseVertical.SetActive( true);
					mouseHorizontal.SetActive(false);
					anim.SetBool("pushingButton", true);
					anim.SetBool("Climbing", true);
					canClimb = true;
					gameObject.layer = 9;
					body.MovePosition (new Vector2(ladder.transform.position.x,body.position.y) + climbSpeed * Time.deltaTime);
					
				} else {
					anim.SetBool("pushingButton", false);
				}
			}else{
				canClimb = false;
				ladder = null;
				rigidbody2D.gravityScale = 1;
			}
		}
	}
	

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Ladder")) {
			ladder = other.gameObject;
		}else if (other.gameObject.CompareTag("Cheese")){
			if (numCheese == 0){
				Destroy(other.gameObject);
				mouseHorizontal.transform.FindChild("Cheese").gameObject.SetActive( true);
				mouseVertical.transform.FindChild("Cheese").gameObject.SetActive(true);
			}
			numCheese++;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Ladder")) {
			anim.SetBool("Climbing", false);
			mouseVertical.gameObject.SetActive( false);
			mouseHorizontal.gameObject.SetActive(true);
			gameObject.rigidbody2D.gravityScale = 1;
			gameObject.rigidbody2D.velocity = new Vector2(0,0);
			gameObject.layer = 0;
			ladder = null;
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if(other.gameObject.CompareTag("Ground")){
			if(mouseTrans.localScale.y == -1)
			mouseTrans.localScale = new Vector3(mouseTrans.localScale.x,1,1);
			isGrounded = true;
			canClimb = true;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if(other.gameObject.CompareTag("Ground")){
			isGrounded = false;
		}
	}
}
