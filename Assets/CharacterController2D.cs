using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	Rigidbody2D body ;
	Collider2D collider;
	bool canClimb = false;
	bool isGrounded = false;
	bool ladderTop = false;
	bool ladderBottom = false;
	Vector2 ladderPos;
	Vector2 walkSpeed = new Vector2 (10,0);
	Vector2 climbSpeed = new Vector2(0,10);
	// Use this for initialization
	void Start () {
		body = this.GetComponent<Rigidbody2D> ();
		collider = this.GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (canClimb == false && isGrounded == false && ladderTop == false) {
			body.isKinematic = false;		
		}else{
			body.isKinematic = true;
		}


		if(isGrounded || !canClimb && ladderTop){
			if(Input.GetKey(KeyCode.A)){
				body.MovePosition(body.position - walkSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.D)){
				body.MovePosition(body.position + walkSpeed * Time.deltaTime);
			}
		}
		if(canClimb){

			if(Input.GetKey(KeyCode.W)){
				isGrounded = false;
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.S)){

				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}
		}else if(ladderTop){
			if (Input.GetKey(KeyCode.S)){
				
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);
			}
		}else if (ladderBottom){
			if (Input.GetKey(KeyCode.W)){
				body.MovePosition(new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
				canClimb = true;
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
			canClimb = true;
			ladderPos = other.transform.position;
		}
		if (other.CompareTag ("LadderTop")) {
			ladderTop = true;
		}
		if (other.CompareTag ("LadderBottom")) {
			ladderBottom = true;
		}

		if(other.CompareTag("Ground"))
		{
			isGrounded = true;
			canClimb = false;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
			isGrounded = true;
			canClimb = false;
		}
		if (other.CompareTag("Ground")){
			isGrounded = false;
		}
		if (other.CompareTag("LadderTop")){
			ladderTop = false;
		}
		if (other.CompareTag("LadderBottom")){
			ladderBottom = false;
		}
	}


}
