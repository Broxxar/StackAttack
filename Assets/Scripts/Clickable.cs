using UnityEngine;
using System.Collections;

public delegate void MouseEventHandler (Vector3 position);

public enum MouseEventType
{
	Down,
	Up,
	RightDown,
	RightUp
}

public class Clickable : MonoBehaviour
{
	public event MouseEventHandler DownAction = delegate {};
	public event MouseEventHandler UpAction = delegate {};
	public event MouseEventHandler RightDownAction = delegate {};
	public event MouseEventHandler RightUpAction = delegate {};
	
	public void FireEvent (MouseEventType t, Vector3 position)
	{
		if (t == MouseEventType.Down)
		{
			DownAction(position);
		}
		else if (t == MouseEventType.Up)
		{
			UpAction(position);
		}
		else if (t == MouseEventType.RightDown)
		{
			RightDownAction(position);
		}
		else if (t == MouseEventType.RightUp)
		{
			RightUpAction(position);
		}
	}
}