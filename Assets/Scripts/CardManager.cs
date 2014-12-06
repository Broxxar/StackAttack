using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour {


	Stack<Card> CardStack = new Stack<Card>();

	void Awake ()
	{
		
		
		
		GetComponent<Clickable>().DownAction += OnDownAction;
		GetComponent<Clickable>().UpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{
		
	//	drag = false;
		
	}
	
	void OnUpAction (Vector3 position)
	{
	//	drag = false;
	}



	public void AddToStack(Card card)
	{
		CardStack.Push(card);
		//card.EnableAllColliders();
	}

	public void RemoveFromStack()
	{

		Card card = CardStack.Pop();
		//card.DisableAllColliders();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
