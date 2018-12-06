using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adjective : Expression
{
	public Enums.descriptors[] descriptors { get ; private set; }

	public Adjective(string adj, Enums.descriptors[] desc)
	{
		expression = adj;
		descriptors = desc;
	}
}
