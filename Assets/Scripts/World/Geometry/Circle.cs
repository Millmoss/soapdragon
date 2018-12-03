using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
	public Vector2Int center { get; private set; }
	public int radius { get; private set; }

	public Circle(Vector2Int c, int r)
	{
		center = c;
		radius = r;
	}

	public bool Within(Vector2Int position)
	{
		if (Vector2Int.Distance(position, center) <= radius)
			return true;
		return false;
	}
}
