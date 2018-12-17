using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences
{
	private Dictionary<string, Dictionary<string, Dictionary<string, float>>> preferences;
	private char[] splitColon;
	private char[] splitPercent;

	public Preferences()
	{
		preferences = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();
	}

	public Preferences(string[] pdp)
	{
		preferences = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();
		splitPercent = new char[1];
		splitPercent[0] = '%';
		splitColon = new char[1];
		splitColon[0] = ':';

		for (int i = 0; i < pdp.Length; i++)
		{
			string[] f = pdp[i].Split(splitColon);
			string[] s = f[0].Split(splitPercent);

			if (s.Length != 3)
			{
				MonoBehaviour.print("ERROR IN FILE CONTENTS");
				continue;
			}

			if (!preferences.ContainsKey(s[0]))
				preferences.Add(s[0], new Dictionary<string, Dictionary<string, float>>());
			if (!preferences[s[0]].ContainsKey(s[1]))
				preferences[s[0]].Add(s[1], new Dictionary<string, float>());
			if (!preferences[s[0]][s[1]].ContainsKey(s[2]))
				preferences[s[0]][s[1]].Add(s[2], float.Parse(f[1]));
		}
	}

	public Dictionary<string, Dictionary<string, Dictionary<string, float>>> data()
	{
		return new Dictionary<string, Dictionary<string, Dictionary<string, float>>>(preferences);
	}

	public bool has(string category)
	{
		return preferences.ContainsKey(category);
	}

	public bool has(string category, string feature)
	{
		if (preferences.ContainsKey(category))
			return preferences.ContainsKey(feature);

		return false;
	}

	public bool has(string category, string feature, string type)
	{
		if (preferences.ContainsKey(category))
			if (preferences[category].ContainsKey(feature))
				return preferences[category][feature].ContainsKey(type);

		return false;
	}

	public float get(string category)
	{
		if (preferences.ContainsKey(category))
		{
			float div = 0;
			float prefAvg = 0;

			foreach (string f in preferences[category].Keys)
			{
				foreach (string t in preferences[category][f].Keys)
				{
					prefAvg += preferences[category][f][t];		//averaging function should be modified
					div++;
				}
			}

			prefAvg = prefAvg / div;
			return prefAvg;
		}

		return 0;
	}

	public float get(string category, string feature)
	{
		if (preferences.ContainsKey(category))
		{
			if (preferences[category].ContainsKey(feature))
			{
				float div = 0;
				float prefAvg = 0;

				foreach (string t in preferences[category][feature].Keys)
				{
					prefAvg += preferences[category][feature][t];	//averaging function should be modified
					div++;
				}

				prefAvg = prefAvg / div;
				return prefAvg;
			}
		}

		return 0;
	}

	public float get(string category, string feature, string type)
	{
		if (preferences.ContainsKey(category))
		{
			if (preferences[category].ContainsKey(feature))
			{
				if (preferences[category][feature].ContainsKey(type))
				{
					return preferences[category][feature][type];
				}
			}
		}

		return 0;
	}

	public void set(string category, string feature, string type, float value)	//should this prevent setting if value already exists?
	{
		if (!preferences.ContainsKey(category))
		{
			preferences.Add(category, new Dictionary<string, Dictionary<string, float>>());
		}
		if (!preferences[category].ContainsKey(feature))
		{
			preferences[category].Add(feature, new Dictionary<string, float>());
		}
		if (!preferences[category][feature].ContainsKey(type))
		{
			preferences[category][feature].Add(type, value);
		}
		else
		{
			preferences[category][feature][type] = value;
		}
	}

	public bool mod(string category, string feature, string type, float value)	//returns true if preference did already exist, false if preference did not previously exist
	{
		if (!preferences.ContainsKey(category))
		{
			preferences.Add(category, new Dictionary<string, Dictionary<string, float>>());
		}
		if (!preferences[category].ContainsKey(feature))
		{
			preferences[category].Add(feature, new Dictionary<string, float>());
		}
		if (!preferences[category][feature].ContainsKey(type))
		{
			preferences[category][feature].Add(type, value);
			return false;
		}
		else
		{
			preferences[category][feature][type] += value;
			preferences[category][feature][type] = Mathf.Clamp(preferences[category][feature][type], -1, 1);
			return true;
		}
	}

	public string getClosestMatchingPerson(Person prsn, float value)
	{
		Dictionary<string, string> fs = prsn.GetFeaturesString();
		string index = "null";
		float dif = 2;
		foreach (string f in fs.Keys)
		{
			if (preferences.ContainsKey("person") && preferences["person"].ContainsKey(f) && preferences["person"][f].ContainsKey(fs[f]))
			{
				float v = preferences["person"][f][fs[f]];
				if (Mathf.Abs(value - v) < dif)
				{
					index = f;
					dif = Mathf.Abs(value - v);
				}
			}
		}
		return index;
	}

	public string getClosestMatchingAny(float value, string exc)
	{
		string indexa = "null";
		string indexb = "null";
		string indexc = "null";
		float dif = 2;
		foreach (string f in preferences.Keys)
		{
			foreach (string ff in preferences[f].Keys)
			{
				if (ff != exc)
				{
					float v = get(f, ff);
					if (Mathf.Abs(value - v) < dif)
					{
						indexa = f;
						indexb = ff;
						dif = Mathf.Abs(value - v);
					}
				}
			}
		}

		dif = 2;
		foreach (string f in preferences[indexa][indexb].Keys)
		{
			float v = preferences[indexa][indexb][f];
			if (Mathf.Abs(value - v) < dif)
			{
				indexc = f;
				dif = Mathf.Abs(value - v);
			}
		}

		return indexa + "%" + indexb + "%" + indexc;
	}
}
