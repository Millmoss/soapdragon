using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verb : Expression      //later this will actually hold info for verbs that actually do things in game
{
	public Enums.descriptors[] descriptors { get; private set; }
	public Noun affects { get; private set; }

	public Verb(string verb, Enums.descriptors[] desc, Noun afcts)
	{
		expression = verb;
		descriptors = desc;
		affects = afcts;
	}
}
