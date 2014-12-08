using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	Vector2 walkSpeed = new Vector2 (3,0);
	Vector2 climbSpeed = new Vector2(0,3);
	Vector2 ladderPos = new Vector2(8000,8000); //the center of the ladder, used to center mouse on climb
	bool isGrounded = false; //to know when mouse can walk
	bool canClimb = false;   
	Rigidbody2D body ;
	Animator anim;
	Transform mouseTrans;
	GameObject mouseVertical, mouseHorizontal;

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
			if (Input.GetKey (KeyCode.A)) {
					body.MovePosition (body.position - walkSpeed * Time.deltaTime);
				anim.SetBool("Running", true);
			} else if (Input.GetKey (KeyCode.D)) {
					body.MovePosition (body.position + walkSpeed * Time.deltaTime);
				anim.SetBool("Running", true);
			} else{
				anim.SetBool("Running", false);
			}
		}
		if(ladderPos.x != 8000){//set to unrealistic number(8000) when mouse leaves trigger
			gameObject.rigidbody2D.gravityScale = 0;

			if (Input.GetKey (KeyCode.S)) {
				mouseVertical.SetActive( true);
				mouseHorizontal.SetActive(false);
				anim.SetBool("pushingButton", true);
				anim.SetBool("Climbing", true);
				canClimb = true;
				gameObject.layer = 9;
				body.MovePosition (new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);

			} else if (Input.GetKey (KeyCode.W)) {
				mouseVertical.SetActive( true);
				mouseHorizontal.SetActive(false);
				anim.SetBool("pushingButton", true);
				anim.SetBool("Climbing", true);
				canClimb = true;
				gameObject.layer = 9;
				body.MovePosition (new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
				
			} else {
				anim.SetBool("pushingButton", false);
			}
		}
	}
	

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Ladder")) {
			ladderPos = other.gameObject.transform.position;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Ladder")) {
			anim.SetBool("Climbing", false);
			mouseVertical.gameObject.SetActive( false);
			mouseHorizontal.gameObject.SetActive(true);
			gameObject.rigidbody2D.gravityScale = 1;
			gameObject.layer = 0;
			ladderPos = new Vector2(8000,8000);
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if(other.gameObject.CompareTag("Ground")){
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if(other.gameObject.CompareTag("Ground")){
			isGrounded = false;
		}
	}
}
