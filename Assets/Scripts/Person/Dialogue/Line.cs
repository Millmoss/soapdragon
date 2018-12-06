using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
	private Expression[] line;
	private HashSet<Enums.descriptors> keys;

	public Line(Expression[] l)
	{
		line = (Expression[]) l.Clone();
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] is Verb)
			{
				for (int k = 0; k < ((Verb)line[i]).descriptors.Length; k++)
					keys.Add(((Verb)line[i]).descriptors[k]);
			}
			else if (line[i] is Adjective)
			{
				for (int k = 0; k < ((Adjective)line[i]).descriptors.Length; k++)
					keys.Add(((Adjective)line[i]).descriptors[k]);
			}
		}
	}

	public string getLineString()
	{
		string s = "";
		for (int i = 0; i < line.Length; i++)
		{
			s += line[i].expression;
		}
		return s;
	}

	public float aggregateLine()
	{
		float feeling = 0;

		IEnumerator<Enums.descriptors> numer = keys.GetEnumerator();
		for (int i = 0; i < keys.Count; i++)
		{
			numer.MoveNext();
			switch (numer.Current)
			{
				case Enums.descriptors.loving:
					{
						feeling += .5f;
						break;
					}
				case Enums.descriptors.friendly:
					{
						feeling += .3f;
						break;
					}
				case Enums.descriptors.agreeable:
					{
						feeling += .4f;
						break;
					}
				case Enums.descriptors.safe:
					{
						feeling += .1f;
						break;
					}
				case Enums.descriptors.hostile:
					{
						feeling += -.4f;
						break;
					}
				case Enums.descriptors.disagreeable:
					{
						feeling += -.3f;
						break;
					}
				default:
					{
						break;
					}
			}
		}

		if (keys.Count < 2)
			feeling *= 2;

		return feeling;
	}
}
