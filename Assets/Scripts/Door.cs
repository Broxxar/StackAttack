using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public Sprite OpenSprite;
	public int CheeseGoal = 0;
	public string NextLevelName;

	CharController2D player;
	SpriteRenderer sr;
	ParticleSystem poof;

	void Start ()
	{
		player = FindObjectOfType (typeof(CharController2D)) as CharController2D;
		sr = GetComponent<SpriteRenderer>();
		poof = GetComponent<ParticleSystem>();
	}
	
	void Open ()
	{
		sr.sprite = OpenSprite;
		poof.Play();
	}	
	
	void OnTriggerStay2D ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && 0 == CheeseGoal)
		{	
			Application.LoadLevel (NextLevelName);
		}
	}
}
