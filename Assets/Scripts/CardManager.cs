using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour
{
	Stack<Card> CardStack = new Stack<Card>();
	int sortOrderDelta = 10;

	void Awake ()
	{
		InputManager.Instance.GlobalDownAction += OnDownAction;
	}
	
	void OnDownAction (Vector3 position)
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D col = Physics2D.OverlapPoint(mousePos,1<<8);
		
		if (col == collider2D)
			RemoveFromStack ();
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
		    card.DisableAllColliders();
			card.AdjustSortOrder(-CardStack.Count * sortOrderDelta);
			card.inStack = false;
			card.Focus();
		}
	}
}
