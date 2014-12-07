using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	Collider2D currentCollision;
	Rigidbody2D body ;
	Animator anim;
	Transform mouseTrans;
	Collider2D collider;
	bool canClimb = false;
	bool isGrounded = false;
	bool ladderTop = false;
	bool ladderBottom = false;
	Vector2 ladderPos;
	Vector2 walkSpeed = new Vector2 (3,0);
	Vector2 climbSpeed = new Vector2(0,3);

	// Use this for initialization
	void Start () {
		mouseTrans = this.transform.FindChild("mouse").GetComponent<Transform>();
		anim = this.GetComponentInChildren<Animator> ();
		body = this.GetComponent<Rigidbody2D> ();
		collider = this.GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((canClimb == false && isGrounded == false && ladderTop == false)|| currentCollision.enabled == false) {
			body.isKinematic = false;		
		}else{
			body.isKinematic = true;
		}

		if(isGrounded || !canClimb && ladderTop){

			if(Input.GetKey(KeyCode.A)){
				anim.SetBool("Running", true);
				mouseTrans.localScale = new Vector3(-1,1,1);
				body.MovePosition(body.position - walkSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.D)){
				anim.SetBool("Running", true);
				mouseTrans.localScale = new Vector3(1,1,1);
				body.MovePosition(body.position + walkSpeed * Time.deltaTime);
			}else{
				anim.SetBool("Running", false);

			}

		}
		if(canClimb){
			//on ladder keep climbing
			if(Input.GetKey(KeyCode.W)){
				anim.SetBool("Climbing", true);
			//	anim.SetBool ("Running", false);
				anim.SetBool ("pushingButton",true);
				mouseTrans.localScale = new Vector3(1,1,1);
				isGrounded = false;
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.S)){
				anim.SetBool("Climbing", true);
			//	anim.SetBool("Running", false);
				anim.SetBool ("pushingButton",true);
				mouseTrans.localScale = new Vector3(1,-1,1);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}else{
				anim.SetBool ("pushingButton",false);
			}
			//if top of ladder only climbdown
		}else if(ladderTop){
			if (Input.GetKey(KeyCode.S)){
				canClimb = true;
				anim.SetBool("Climbing", true);
				mouseTrans.localScale = new Vector3(1,-1,1);
				anim.SetBool ("pushingButton",true);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}
			//if bottom of ladder only climb up
		}else if (ladderBottom){
			if (Input.GetKey(KeyCode.W)){
				anim.SetBool("Climbing", true);
				mouseTrans.localScale = new Vector3(1,1,1);
				anim.SetBool ("pushingButton",true);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
				canClimb = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.CompareTag("Ground")){
			currentCollision = other;
			isGrounded = true;
			canClimb = false;
			anim.SetBool("Climbing",false);
			this.transform.localScale = new Vector3 (this.transform.localScale.x, 1, 1);

		}if (other.CompareTag ("Ladder")) {
			currentCollision = other;		
		}
	}
	void OnTriggerEnter2D(Collider2D other){


		if (other.CompareTag ("Ladder")) {
			//needed for centering mouse

		
		}
		if (other.CompareTag ("LadderTop")) {
			ladderPos = other.transform.position;
			ladderTop = true;
		}
		if (other.CompareTag ("LadderBottom")) {
			ladderPos = other.transform.position;
			ladderBottom = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Ground")) {
			isGrounded = false;
		}else if (other.CompareTag ("Ladder")) {
			//isGrounded = true;
			canClimb = false;
			anim.SetBool("Climbing",false);
			this.transform.localScale = new Vector3 (this.transform.localScale.x, 1, 1);

		}else if (other.CompareTag("LadderTop")){
			ladderTop = false;
		}else if (other.CompareTag("LadderBottom")){
			ladderBottom = false;
		}
	}


}
