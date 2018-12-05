using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing	//I wanted to call it object but obviously that's not a good idea
{

	public string name { get; private set; }
	public Vector2Int position { get; set; }
	public int weight { get; private set; }
	public int durability { get; private set; }	//goes down by 1 if use causes damage. high for things like anvils, low for food and glass
	public int nutrition { get; private set; }	//the nutritional value of an object, can be negative

	public float effect { get; private set; }	//effectiveness. like how well it cooks, how good it tastes, how much it heals. context specific
	public int useTime { get; private set; }	//time it takes to use it

	private HashSet<Enums.uses> useKeys;            //all use keywords for what this object is used for (food, cook, store, damage, heal, etc)
	private HashSet<Enums.constraints> constraintKeys;     //all restricting keywords for this object (stationary, indestructable, etc(?))
	private HashSet<string> featureKeys;		//all description keywords for this object (tasty, yellow, spiky, iron, stupid, dumb, i don't like it, i hate it, etc)

	public Thing(string nm, Vector2Int pos, int wt, int dur, int nut, float eff, int utm, HashSet<Enums.uses> ukey, HashSet<Enums.constraints> ckey, HashSet<string> fkey)
	{
		name = nm;
		position = pos;
		weight = wt;
		durability = dur;
		nutrition = nut;
		effect = eff;
		useTime = utm;
		useKeys = ukey;
		constraintKeys = ckey;
		featureKeys = fkey;
	}


    public void Damage()
    {
        durability--;
        Debug.Log(durability);
    }

	public bool hasUse(Enums.uses use)
	{
		return useKeys.Contains(use);
	}

	public HashSet<Enums.uses> uses
	{
		get { return new HashSet<Enums.uses>(useKeys); }
	}

	public bool hasConstraint(Enums.constraints constraint)
	{
		return constraintKeys.Contains(constraint);
	}

	public HashSet<Enums.constraints> constraints
	{
		get { return new HashSet<Enums.constraints>(constraintKeys); }
	}

	public bool hasFeature(string feature)
	{
		return featureKeys.Contains(feature);
	}

	public HashSet<string> features
	{
		get { return new HashSet<string>(featureKeys); }
	}
}
