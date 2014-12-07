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
	
	// Use this for initialization
	void Start () {
		mouseTrans = this.transform.FindChild("mouse").GetComponent<Transform>();
		anim = this.GetComponentInChildren<Animator> ();
		body = this.GetComponent<Rigidbody2D> ();
		collider = this.GetComponent<Collider2D> ();
	}

	
	
	// Update is called once per frame
	void Update () {
		if (fall||(!isGrounded&&!canClimb&&!ladderTop)) {
			isGrounded = false;
			body.isKinematic = false;		
		}else{
			body.isKinematic = true;
		}

		
		
		if(isGrounded || !canClimb && ladderTop){
			
			if(Input.GetKey(KeyCode.A)){
				//anim.SetBool("Running", true);
				//mouseTrans.localScale = new Vector3(-1,1,1);
				body.MovePosition(body.position - walkSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.D)){
				//anim.SetBool("Running", true);
				//mouseTrans.localScale = new Vector3(1,1,1);
				body.MovePosition(body.position + walkSpeed * Time.deltaTime);
			}else{
				//anim.SetBool("Running", false);
				
			}
			
		}
		if(canClimb){
			//on ladder keep climbing
			if(Input.GetKey(KeyCode.W)){
				//anim.SetBool("Climbing", true);
				//	anim.SetBool ("Running", false);
				//anim.SetBool ("pushingButton",true);
				//mouseTrans.localScale = new Vector3(1,1,1);
				isGrounded = false;
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.S)){
				//anim.SetBool("Climbing", true);
				//anim.SetBool ("pushingButton",true);
				//mouseTrans.localScale = new Vector3(1,-1,1);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}else{
		//		anim.SetBool ("pushingButton",false);
			}
			//if top of ladder only climbdown
		}else if(ladderTop){
			if (Input.GetKey(KeyCode.S)){
				canClimb = true;
				//anim.SetBool("Climbing", true);
				//mouseTrans.localScale = new Vector3(1,-1,1);
				//anim.SetBool ("pushingButton",true);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}
			//if bottom of ladder only climb up
		}else if (ladderBottom){
			if (Input.GetKey(KeyCode.W)){
				//anim.SetBool("Climbing", true);
				//mouseTrans.localScale = new Vector3(1,1,1);
				//anim.SetBool ("pushingButton",true);
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
				canClimb = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
			canClimb = false;
		}else if(other.CompareTag ("LadderTop")) {
			ladderTop = false;
		}else if(other.CompareTag ("LadderBottom")) {
			ladderBottom = false;
		}else if (other.CompareTag("Ground")){
			isGrounded = false;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(!other.CompareTag("Manager")){
			bool touchingLevel = false;
			if (other.CompareTag ("Ground")) {
				touchingLevel = true;
				isGrounded = true;
				canClimb = false;
			}else if (other.CompareTag("Ladder")) {
				touchingLevel = true;
			} else if (other.CompareTag("LadderBottom")) {
				//print ("bottom");
				ladderPos = other.transform.position;
				touchingLevel = true;
				ladderBottom = true;
			} else if (other.CompareTag("LadderTop")) {
				ladderPos = other.transform.position;
				touchingLevel = true;
				ladderTop = true;
			} 
			fall = !touchingLevel;
		}else{
			isGrounded = false;
		}
	}
}
