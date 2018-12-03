using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle
{
	public int xExtent { get; private set; }
	public int yExtent { get; private set; }
	public int rotation { get; private set; }
	public Vector2Int center { get; private set; }

	public Rectangle(Vector2Int c, int x, int y, int r)	//rotation should be zero for now to keep things simple
	{
		center = c;
		xExtent = x;
		yExtent = y;
		rotation = r;
	}

	public bool Within(Vector2Int position)
	{
		if (position.x < center.x + xExtent && position.x > center.x - xExtent &&
			position.y < center.y + yExtent && position.y > center.y - yExtent)
			return true;
		return false;
	}
}
