using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{
	//physical space
	private Rectangle[] rectangles;
	private Circle[] circles;

    //features
    //private Thing[] things;

    //Items and People in the room; <Position_Of_Object, List_Of_Objects_At_Position>
    private Dictionary<Vector2Int, List<Person>> people = new Dictionary<Vector2Int, List<Person>>();
    private Dictionary<Vector2Int, List<Thing>> things = new Dictionary<Vector2Int, List<Thing>>();
    public string name { get; private set; }

    public Place(Rectangle r, string _name)
	{
		rectangles = new Rectangle[1];
		rectangles[0] = r;
		circles = null;
        name = _name;
	}

	public Place(Rectangle[] rs, string _name)
	{
		rectangles = rs;
		circles = null;
        name = _name;
    }

	public Place(Circle[] cs, string _name)
	{
		rectangles = null;
		circles = cs;
        name = _name;
    }

	public Place(Rectangle[] rs, Circle[] cs, string _name)	//DO NOT USE FOR NOW
	{
		rectangles = rs;
		circles = cs;
        name = _name;
    }

    //Check if the given position is inside the place; e.g. it fits within any of the given circles / rectangles.
	public bool At(Vector2Int position)
	{
		if (rectangles != null)
		{
			for (int i = 0; i < rectangles.Length; i++)
				if (rectangles[i].Within(position))
					return true;
		}
		if (circles != null)
		{
			for (int i = 0; i < circles.Length; i++)
				if (circles[i].Within(position))
					return true;
		}

		return false;
	}

    //Updates every person within this room. If this isn't a good place for this function, feel free to move it.
    public void PeopleUpdate()
    {
        foreach (List<Person> x in people.Values)
            foreach (Person y in x)
            {
                y.Update(people, things);
            }
    }

    //Activates every person within this room, and returns a string that
    //contains what each eprson did as a result of their actions.  
    //If this isn't a good place for this function, feel free to move it.
    public string PeopleActivate()
    {
        string ret = "";
        foreach (List<Person> x in people.Values)
            foreach (Person y in x)
            { 
                ret += y.Action();
                ret += "\n";
            }
        ret = ret.Substring(0, ret.Length - 1);
        return ret;
    }

    //Add things to a position
    public void AddThings(Vector2Int _pos, List<Thing> _things)
    {
        if (!things.ContainsKey(_pos))
            things[_pos] = _things;
        else
            foreach (Thing x in _things)
                things[_pos].Add(x);
    }

    //Add a thing to a position
    public void AddThing(Thing _thing)
    {
        if (!things.ContainsKey(_thing.position))
            things[_thing.position] = new List<Thing>();
            things[_thing.position].Add(_thing);
    }

    //Add people to a position
    public void AddPeople(Vector2Int _pos, List<Person> _ppl)
    {
        people[_pos] = _ppl;
    }

    //Remove a thing at a specific position.
    public void RemoveThing(Vector2Int _pos)
    {
        things.Remove(_pos);
    }

    
}
