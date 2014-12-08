using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	Vector2 walkSpeed = new Vector2 (3,0);
	Vector2 climbSpeed = new Vector2(0,3);
	Vector2 ladderPos = new Vector2(8000,8000); //the center of the ladder, used to center mouse on climb
	bool isGrounded = false; //to know when mouse can walk
	bool canClimb = false;   
	Rigidbody2D body ;

	void Start(){
		body = GetComponent<Rigidbody2D> ();
	}


	void FixedUpdate(){
		print (isGrounded);
		if (isGrounded) {//only want him to move sideways when on a platform
			if (Input.GetKey (KeyCode.A)) {
					body.MovePosition (body.position - walkSpeed * Time.deltaTime);

			} else if (Input.GetKey (KeyCode.D)) {
					body.MovePosition (body.position + walkSpeed * Time.deltaTime);

			} 
		}
		if(ladderPos.x != 8000){//set to unrealistic number(8000) when mouse leaves trigger
			gameObject.rigidbody2D.gravityScale = 0;

			if (Input.GetKey (KeyCode.S)) {
				canClimb = true;
				gameObject.layer = 9;
				body.MovePosition (new Vector2(ladderPos.x,body.position.y) - climbSpeed * Time.deltaTime);

			} else if (Input.GetKey (KeyCode.W)) {
				canClimb = true;
				gameObject.layer = 9;
				body.MovePosition (new Vector2(ladderPos.x,body.position.y) + climbSpeed * Time.deltaTime);
				
			} 
		}
	}




	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Ladder")) {
			//canClimb = true;	
			ladderPos = other.gameObject.transform.position;
			//gameObject.layer = 9;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Ladder")) {
			gameObject.rigidbody2D.gravityScale = 1;
			canClimb = false;	
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
