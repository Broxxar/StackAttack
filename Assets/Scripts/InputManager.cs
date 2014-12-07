using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	private static InputManager _instance;
	
	public event MouseEventHandler GlobalDownAction = delegate {};
	public event MouseEventHandler GlobalUpAction = delegate {};
	public event MouseEventHandler GlobalRightDownAction = delegate {};
	public event MouseEventHandler GlobalRightUpAction = delegate {};
	
	public static InputManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<InputManager>();
			}
			return _instance;
		}
		
		private set { }
	}
	
	public Vector2 MouseWorldPosition {get; private set;}
	
	void Update ()
	{
		MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if (Input.GetMouseButtonDown(0))
		{
			Collider2D collider = Physics2D.OverlapPoint(MouseWorldPosition);
			GlobalDownAction(MouseWorldPosition);
			
			if (collider != null)
			{
				Clickable clickable = collider.GetComponent<Clickable>();
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.Down, MouseWorldPosition);
				}
			}else{
				Clickable clickable = Camera.main.GetComponent<Clickable>();
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.Down, MouseWorldPosition);
				}
			}
		}
		
		else if (Input.GetMouseButtonUp(0))
		{
			Collider2D collider = Physics2D.OverlapPoint(MouseWorldPosition);
			GlobalUpAction(MouseWorldPosition);
			
			if (collider != null)
			{
				Clickable clickable = collider.GetComponent<Clickable>();	
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.Up, MouseWorldPosition);
				}
			}else{
				Clickable clickable = Camera.main.GetComponent<Clickable>();
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.Up, MouseWorldPosition);
				}
			}
		}

		else if (Input.GetMouseButtonDown(1))
		{
			Collider2D collider = Physics2D.OverlapPoint(MouseWorldPosition);
			GlobalRightDownAction(MouseWorldPosition);
			
			if (collider != null)
			{
				Clickable clickable = collider.GetComponent<Clickable>();	
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.RightDown, MouseWorldPosition);
				}
			}else{
				Clickable clickable = Camera.main.GetComponent<Clickable>();
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.RightDown, MouseWorldPosition);
				}
			}
		}
		else if (Input.GetMouseButtonUp(1))
		{
			Collider2D collider = Physics2D.OverlapPoint(MouseWorldPosition);
			GlobalRightUpAction(MouseWorldPosition);
			
			if (collider != null)
			{
				Clickable clickable = collider.GetComponent<Clickable>();	
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.RightUp, MouseWorldPosition);
				}
			}else{
				Clickable clickable = Camera.main.GetComponent<Clickable>();
				if (clickable != null)
				{
					clickable.FireEvent(MouseEventType.RightUp, MouseWorldPosition);
				}
			}
		}

	}
}