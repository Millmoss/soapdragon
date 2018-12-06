using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expression
{
	public string expression { get; protected set; }

	public Expression()
	{

	}

	public Expression(string e)
	{
		expression = e;
	}
}
