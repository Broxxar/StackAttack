using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour {


	Stack<Card> CardStack = new Stack<Card>();
	int sortOrderDelta = 10;

	void Awake ()
	{
		InputManager.Instance.GlobalDownAction += OnDownAction;
		InputManager.Instance.GlobalUpAction += OnUpAction;
	}
	
	void OnDownAction (Vector3 position)
	{
		SpriteRenderer temp = this.gameObject.GetComponent<SpriteRenderer> ();
		print (gameObject.tag);
		float right = temp.bounds.max.x;
		float left = temp.bounds.min.x;
		float top =temp.bounds.max.y;
		float bottom = temp.bounds.min.y;
		print (right+","+left+","+top+","+bottom);
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		print (mousePos);
		if ((mousePos.x < right && mousePos.x > left) && (mousePos.y < top && mousePos.y > bottom)) {
			print("remove");
			RemoveFromStack ();
		}

	}
	
	void OnUpAction (Vector3 position)
	{
		//drag = false;
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
