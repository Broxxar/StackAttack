using UnityEngine;
using System.Collections;

public class CharController2D : MonoBehaviour {
	Rigidbody2D body ;
	Animator anim;
	Transform mouseTrans;
	Collider2D collider;
	bool canClimb = false;
	bool isGrounded = false;
	bool ladderTop = false;
	bool ladderBottom = false;
	bool fall = true;
	Vector2 ladderPos;
	Vector2 walkSpeed = new Vector2 (3,0);
	Vector2 climbSpeed = new Vector2(0,3);
	Collider2D currentCollision ;
	
	// Use this for initialization
	void Start () {
		mouseTrans = this.transform.FindChild("mouse").GetComponent<Transform>();
		anim = this.GetComponentInChildren<Animator> ();
		body = this.GetComponent<Rigidbody2D> ();
		collider = this.GetComponent<Collider2D> ();
	}

	
	
	// Update is called once per frame
	void Update () {
		if ((currentCollision!=null && currentCollision.enabled == false)||(!isGrounded&&!canClimb&&!ladderTop)) {
			isGrounded = false;
			canClimb = false;
			ladderTop = false;
			ladderBottom = false;
			body.isKinematic = false;
		}else{
			body.isKinematic = true;
		}
		
		
		if(isGrounded || !canClimb && ladderTop){
			
			if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)){
				anim.SetBool("Running", true);
				anim.SetBool ("Climbing", false);
				mouseTrans.localScale = new Vector3(-1,1,1);
				body.MovePosition(body.position - walkSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)){
				anim.SetBool("Running", true);
				anim.SetBool("Climbing",false);
				mouseTrans.localScale = new Vector3(1,1,1);
				body.MovePosition(body.position + walkSpeed * Time.deltaTime);
			}else{
				anim.SetBool("Running", false);
				
			}
			
		}
		if(canClimb){
			//on ladder keep climbing
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)){
				anim.SetBool("Climbing", true);
				anim.SetBool ("Running", false);
				anim.SetBool ("pushingButton",true);
				mouseTrans.localScale = new Vector3(1,1,1);
				isGrounded = false;
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)){
				anim.SetBool("Climbing", true);
				anim.SetBool ("pushingButton",true);
				mouseTrans.localScale = new Vector3(1,-1,1);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}else{
				anim.SetBool ("pushingButton",false);
			}
			//if top of ladder only climbdown
		}else if(ladderTop){
			if (Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)){
				canClimb = true;
				anim.SetBool("Climbing", true);
				mouseTrans.localScale = new Vector3(1,-1,1);
				anim.SetBool ("pushingButton",true);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}
			//if bottom of ladder only climb up
		}else if (ladderBottom){
			if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)){
				anim.SetBool("Climbing", true);
				mouseTrans.localScale = new Vector3(1,1,1);
				anim.SetBool ("pushingButton",true);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
				canClimb = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
			body.isKinematic = false;
		}if(other.CompareTag("Ground")){
			body.isKinematic = false;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
			canClimb = false;
			anim.SetBool("Climbing",false);
		}else if(other.CompareTag ("LadderTop")) {
			ladderTop = false;
		}else if(other.CompareTag ("LadderBottom")) {
			ladderBottom = false;
		}else if (other.CompareTag("Ground")){
			isGrounded = false;
		}
	}

	void OnTriggerStay2D(Collider2D other){


			if (other.CompareTag ("Ground")) {
				
				isGrounded = true;
				canClimb = false;
			 
				currentCollision = other;
				anim.SetBool("Climbing",false);
				anim.SetBool("Running", true);
				mouseTrans.localScale = new Vector3(mouseTrans.localScale.x, 1, 1);
			}else if (other.CompareTag("Ladder")) {
				
				currentCollision = other;
			} else if (other.CompareTag("LadderBottom")) {
				ladderPos = other.transform.position;
				ladderBottom = true;
			} else if (other.CompareTag("LadderTop")) {
				ladderPos = other.transform.position;
				ladderTop = true;
			} 

	}
}
