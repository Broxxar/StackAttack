public class IntVector2 {

	private int X, Y;

	public int x
	{
		get { return X; }
		set { X = value; }
	}

	public int y
	{
		get { return Y; }
		set { Y = value; }
	}

	public IntVector2(int X, int Y){
		this.X = X;
		this.Y = Y;
	}

	// overload operator + 
	public static IntVector2 operator +(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x+b.x, a.y+b.y);
	}


	// overload operator - 
	public static IntVector2 operator -(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x-b.x, a.y-b.y);
	}
}
