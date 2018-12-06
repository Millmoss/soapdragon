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


	public List<string> getLineString(Enums.lineTypes type)
	{
		string line = "";

		switch (type)
		{
				case Enums.lineTypes.opinionDirected:
				{
					int size = opinionDirected.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = opinionDirected[index];
					break;
				}
				case Enums.lineTypes.opinionUndirected:
				{
					int size = opinionUndirected.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = opinionUndirected[index];
					break;
				}
				case Enums.lineTypes.requestDirected:
				{
					int size = requestDirected.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = requestDirected[index];
					break;
				}
				case Enums.lineTypes.insultDirected:
				{
					int size = insultDirected.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = insultDirected[index];
					break;
				}
				case Enums.lineTypes.threatDirected:
				{
					int size = threatDirected.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = threatDirected[index];
					break;
				}
				case Enums.lineTypes.opinionOpinionSimple:
				{
					int size = opinionOpinionSimple.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = opinionOpinionSimple[index];
					break;
				}
				case Enums.lineTypes.questionReason:
				{
					int size = questionReason.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = questionReason[index];
					break;
				}
				case Enums.lineTypes.answerRequest:
				{
					int size = answerRequest.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = answerRequest[index];
					break;
				}
				case Enums.lineTypes.answerDanger:
				{
					int size = answerDanger.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = answerDanger[index];
					break;
				}
				default:
				{
					break;
				}
		}

		List<string> rLine = new List<string>();
		int newStart = 0;
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == '.')
			{
				if (line[newStart] != '.')
				{
					rLine.Add(line.Substring(newStart, i - newStart));
					newStart = i;
				}
				else if (i + 1 >= line.Length)
				{
					rLine.Add(line.Substring(newStart, i - newStart));
					rLine.Add(line.Substring(i, 1));
				}
			}
			else if (line[i] == ' ' || line[i] == ',' || line[i] == '\'' || line[i] == '?')
			{
				if (line[newStart] == '.')
				{
					rLine.Add(line.Substring(newStart, i - newStart));
					newStart = i;
				}
			}
		}

		return rLine;
	}
}
