using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	Rigidbody2D body ;
	Collider2D collider;
	bool canClimb = false;
	bool isGrounded = false;
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
		if (canClimb == false && isGrounded == false) {
			body.isKinematic = false;		
		}else{
			body.isKinematic = true;
		}


		if(isGrounded){
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
		}
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
			canClimb = true;
		
			ladderPos = other.transform.position;
		}
		//check that collisions on the bottom
		if(other.CompareTag("Ground"))
		{
				isGrounded = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Ladder")) {
				canClimb = false;
				
		}
		if (other.CompareTag("Ground")){
				isGrounded = false;
		}
	}

//	//set grounded
//	void OnCollisionEnter2D( Collision2D theCollision)
//	{
//		
//		//check that collisions on the bottom
//		if(theCollision.contacts.Length > 0)
//		{
//			ContactPoint2D contact = theCollision.contacts[0];
//			if(Vector2.Dot(contact.normal, Vector2.up) > 0.5)
//			{
//				isGrounded = true;
//			}
//		}
//		
//	}
//	
//	void OnCollisionExit2D( Collision2D theCollision)
//	{
//		print ("exit");
//		//isGrounded = false;
//		
//}


}
