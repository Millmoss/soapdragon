using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Noun
{
	public string noun { get; private set; }
	public string type { get; private set; }	//feature, person, thing, etc
	public string owner { get; private set; }   //if it is a feature, this would be person or thing, if it is a thing held by person, owner would be person, and so on

	public Noun(string n, string t, string o)
	{
		noun = n;
		type = t;
		owner = o;
	}
}
