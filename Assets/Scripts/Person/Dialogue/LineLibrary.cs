using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineLibrary
{
	public string[] opinionDirected;
	public string[] opinionUndirected;
	public string[] requestDirected;
	public string[] insultDirected;
	public string[] threatDirected;
	public string[] opinionOpinionSimple;
	public string[] questionReason;
	public string[] answerRequest;
	public string[] answerDanger;

	public static LineLibrary CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<LineLibrary>(jsonString);
	}

	public Line getLine(string type)
	{

	}
}
