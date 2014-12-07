using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour {


	Stack<Card> CardStack = new Stack<Card>();
	int sortOrderDelta = 10;

	void Awake ()
	{
		GetComponent<Clickable>().DownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{
		RemoveFromStack ();
	}
	
	void OnUpAction (Vector3 position)
	{
	//	drag = false;
	}



	public void AddToStack(Card card)
	{
		CardStack.Push(card);
		card.EnableAllColliders();
		card.AdjustSortOrder((CardStack.Count - 1) * sortOrderDelta);

	}

	public void RemoveFromStack()
	{
		if (CardStack.Count > 0)
		{
		    Card card = CardStack.Pop();
			//card.GetComponent<Clickable>().DownAction += OnDownAction;
		    card.DisableAllColliders();
			card.AdjustSortOrder(-CardStack.Count * sortOrderDelta);
			card.inStack = false;
			card.Focus();
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
