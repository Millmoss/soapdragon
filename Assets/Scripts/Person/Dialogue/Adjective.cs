using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Adjective
{
	public string adjective { get; private set; }
	public Enums.descriptors[] descriptors { get ; private set; }

	public Adjective(string a, Enums.descriptors[] d)
	{
		adjective = a;
		descriptors = d;
	}
}
