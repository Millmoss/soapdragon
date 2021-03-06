﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
	private Expression[] line;
	private HashSet<Enums.descriptors> keys;
	private Noun about;
	public Enums.lineTypes type { get; private set; }

	public Line(Expression[] l, Enums.lineTypes t, Noun about)
	{
		type = t;
		line = l;
		keys = new HashSet<Enums.descriptors>();

		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] is Verb)
			{
				if (((Verb)line[i]).descriptors.Length > 0 && ((Verb)line[i]) != null)
					for (int k = 0; k < ((Verb)line[i]).descriptors.Length; k++)
						keys.Add(((Verb)line[i]).descriptors[k]);
			}
			else if (line[i] is Adjective)
			{
				if (((Adjective)line[i]).descriptors.Length > 0 && ((Adjective)line[i]) != null)
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

		if (type == Enums.lineTypes.threatDirected)
			feeling = -1f;
		else if (type == Enums.lineTypes.insultDirected)
			feeling = -.6f;

		return feeling;
	}
}
