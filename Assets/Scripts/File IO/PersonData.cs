using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonData
{
	public int xPosition;
	public int yPosition;
	public int rotation;

	public string name;

	//physical

	public int eyesight;
	public float muscle;
	public float fat;
	public float height;
	public float weight;
	public float beauty;

	//mental

	public float wit;
	public float chill;
	public float introversion;

	//emotion

	public float happiness;
	public float sadness;
	public float anger;
	public float fear;
	public float disgust;

	//motivators

	public float hunger;
	public float thirst;
	public float tiredness;
	public float social;
	public float stress;
	public float libido;

	public string[] features;

	public string[] preferences;
	
	public static PersonData CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<PersonData>(jsonString);
	}
}
