using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineLibrary
{
	public string[] opinionDirected;
	public string[] opinionUndirectedPerson;
	public string[] opinionUndirectedOther;
	public string[] requestDirected;
	public string[] insultDirected;
	public string[] threatDirected;
	public string[] opinionOpinionSimple;
	public string[] questionReason;
	public string[] answerRequest;
	public string[] answerDanger;
	public string[] greeting;

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
				case Enums.lineTypes.opinionUndirectedPerson:
				{
					int size = opinionUndirectedPerson.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = opinionUndirectedPerson[index];
					break;
				}
				case Enums.lineTypes.opinionUndirectedOther:
				{
					int size = opinionUndirectedOther.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = opinionUndirectedOther[index];
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
				case Enums.lineTypes.greeting:
				{
					int size = greeting.Length;
					int index = Mathf.FloorToInt(Random.value * size);
					line = greeting[index];
					break;
				}
				default:
				{
					break;
				}
		}

		List<string> rLine = new List<string>();
		int start = 0;

		for (int i = 1; i < line.Length; i++)
		{
			if (line[i] == '[' && line[i + 1] == '%')
			{
				//Main.print(line.Substring(start, i - start));
				rLine.Add(line.Substring(start, i - start));
				start = i;
			}
			if (line[i] == ']' && line[i - 1] == '%')
			{
				//Main.print(line.Substring(start + 2, (i - start) - 3));
				rLine.Add(line.Substring(start + 1, (i - start) - 2));
				start = i + 1;
			}
		}

		if (start != line.Length)
		{
			//Main.print(line.Substring(start, line.Length - start));
			rLine.Add(line.Substring(start, line.Length - start));
		}

		return rLine;
	}
}
