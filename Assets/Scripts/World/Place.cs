using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{
	//physical space
	private Rectangle[] rectangles;
	private Circle[] circles;

	//features
	private Thing[] things;

	public Place(Rectangle r)
	{
		rectangles = new Rectangle[1];
		rectangles[0] = r;
		circles = null;
	}

	public Place(Rectangle[] rs)
	{
		rectangles = rs;
		circles = null;
	}

	public Place(Circle[] cs)
	{
		rectangles = null;
		circles = cs;
	}

	public Place(Rectangle[] rs, Circle[] cs)	//DO NOT USE FOR NOW
	{
		rectangles = rs;
		circles = cs;
	}

	public bool At(Vector2Int position)
	{
		if (rectangles != null)
		{
			for (int i = 0; i < rectangles.Length; i++)
				if (rectangles[i].Within(position))
					return true;
		}
		if (circles != null)
		{
			for (int i = 0; i < circles.Length; i++)
				if (circles[i].Within(position))
					return true;
		}

		return false;
	}
}
