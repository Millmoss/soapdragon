using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Verb		//later this will actually hold info for verbs that actually do things in game
{
	public string verb { get; private set; }

	public Verb(string v)
	{
		verb = v;
	}
}
