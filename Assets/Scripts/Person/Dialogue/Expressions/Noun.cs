using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noun : Expression
{
	public string type { get; private set; }	//feature, person, thing, etc
	public string owner { get; private set; }   //if it is a feature, this would be person or thing, if it is a thing held by person, owner would be person, and so on

	public Noun(string noun, string typ, string ownr)
	{
		expression = noun;
		type = typ;
		owner = ownr;
	}
}
