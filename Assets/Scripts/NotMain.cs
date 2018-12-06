using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotMain : MonoBehaviour
{
	//this is not main! use this for testing garbage, never actually use
	Conversation c;

	void Start ()
	{
		c = new Conversation();

		/*
		Person p1 = new Person("waldo", Enums.gender.Male, new Vector2Int(0, 0), null);
		Person p2 = new Person("odlaw", Enums.gender.Nonbinary, new Vector2Int(0, 0), null);

		Line l1 = c.speak(p1, p2, p2, .7f, -1);
		print(l1.getLineString());
		Line l2 = c.speak(p2, p1, l1, l1.aggregateLine() - 1.3f, -1);
		print(l2.getLineString());
		Line l3 = c.speak(p1, p2, l2, l2.aggregateLine(), -1);
		print(l3.getLineString());
		Line l4 = c.speak(p2, p1, l3, l3.aggregateLine(), -1);
		print(l4.getLineString());
		*/

		Person billiam = new Person("Billiam", Enums.gender.Male, new Vector2Int(0, 0), null,);



		Person janine = new Person("Janine", Enums.gender.Female, new Vector2Int(0, 0), null);



		Person bananaMan = new Person("The Banana Man", Enums.gender.Nonbinary, new Vector2Int(0, 0), null);




	}
	
	void Update ()
	{
		
	}
}
