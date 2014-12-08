using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public Sprite OpenSprite;
	public int CheeseGoal = 0;
	public string NextLevelName;

	CharacterController2D player;
	SpriteRenderer sr;
	ParticleSystem poof;
	bool open;

	void Start ()
	{
		player = FindObjectOfType (typeof(CharacterController2D)) as CharacterController2D;
		sr = GetComponent<SpriteRenderer>();
		poof = GetComponent<ParticleSystem>();
		
		if (CheeseGoal == 0)
		{
			open = true;
			sr.sprite = OpenSprite;
		}
	}
	
	void Open ()
	{
		open = true;
		sr.sprite = OpenSprite;
		poof.Play();
	}	
	
	void OnTriggerStay2D ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && open)
			Application.LoadLevel (NextLevelName);
	}
	
	void Update ()
	{
		if (player.numCheese >= CheeseGoal && !open)
			Open();	
	}
}
