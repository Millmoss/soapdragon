using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adverb : Expression
{
	public float amount { get; private set; }
	public Expression modifies { get; private set; }

	public Adverb(string adv, float amt, Expression mods)
	{
		expression = adv;
		amount = amt;
		modifies = mods;
	}
}
