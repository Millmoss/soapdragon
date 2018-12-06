using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotMain : MonoBehaviour
{
	//this is not main! use this for testing garbage, never actually use
	LineLibrary ll;
	ExpressionLibrary el;

	void Start ()
	{
		ll = FileManager.initLines();
		List<string> lList = ll.getLineString(Enums.lineTypes.opinionDirected);
		for (int i = 0; i < lList.Count; i++)
		{
			//print(lList[i]);
		}
		
		el = FileManager.initExpressions();
		string express = el.getExpressionString(Enums.expressionTypes.feelingVerb, .9f, -1f);
	}
	
	void Update ()
	{
		
	}
}
