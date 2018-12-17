using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory {


    //What rooms are associated with what actions.
    public Dictionary<Place,List<Enums.actions>> locations_assoc;

    //What items are associated with what actions.
    public Dictionary<string, List<Enums.actions>> items_assoc;

    //What we remember about specific objects. Place name -> memory_thing
    public Dictionary<string, List<memory_thing>> remember_items;

    //What we remember about people. memory_person.
    public Dictionary<string, List<memory_person>> remember_people;

    //Last thing we remember a person saying to us.
    public List<KeyValuePair<Person, Line>> lines_said;

    public Memory()
    {
        locations_assoc = new Dictionary<Place, List<Enums.actions>>();
        items_assoc = new Dictionary<string, List<Enums.actions>>();
        remember_items = new Dictionary<string, List<memory_thing>>();
        remember_people = new Dictionary<string, List<memory_person>>();
		lines_said = new List<KeyValuePair<Person, Line>>();
	}

    public void AddLine(Person p, Line l)
    {
		lines_said.Add(new KeyValuePair<Person, Line>(p, l));
		if (lines_said.Count > 5)
			lines_said.RemoveAt(0);
    }

    //Given a person, returns the last line they said to them. Returns null if there is no memory.
    public Line GetLine(Person p)
    {
		for (int i = lines_said.Count; i > 0; i--)
		{
			if (lines_said[i - 1].Key == p)
				return lines_said[i - 1].Value;
		}

		return null;
    }

	public void WipeLines()
	{
		lines_said = new List<KeyValuePair<Person, Line>>();
	}

	public string DetermineAppropriateLine()
	{
		if (lines_said.Count == 0)
			return "greeting";

		if (lines_said[lines_said.Count - 1].Value.type == Enums.lineTypes.greeting)
		{
			if (lines_said.Count == 1)
				return "greeting";

			if (lines_said[lines_said.Count - 2].Value.type != Enums.lineTypes.greeting)
				return "greeting";
		}

		return "normal";
	}

    public List<Thing> GetThings(string place_name)
    {
        List<Thing> t = new List<Thing>();
        if (remember_items.ContainsKey(place_name))
        {
            foreach (memory_thing x in remember_items[place_name])
            {
                t.Add(x.thing);
            }
        }
        return t;
    }

    public Person GetRandomPerson(Place rm)
    {
        if (remember_people.ContainsKey(rm.name) == false)
            return null;
        if (remember_people[rm.name].Count <= 0)
            return null;
        int rand = Random.Range(0, remember_people[rm.name].Count);
        return remember_people[rm.name][rand].person;
    }

    //Add a person to the memory of people. 
    public void AddPerson(string place_name, Vector2Int pos, Person obj)
    {
        memory_person tmp = new memory_person();
        tmp.pos_at_place = pos;
        tmp.person = obj;
        if (remember_people.ContainsKey(place_name))
        {
            remember_people[place_name].Add(tmp);
        }
        else
        {
            remember_people[place_name] = new List<memory_person>() { tmp };
        }
    }

    //Add a thing to the memory of things. 
    public void AddThing(string place_name, Vector2Int pos, Thing obj)
    {
        memory_thing tmp = new memory_thing();
        tmp.pos_at_place = pos;
        tmp.thing = obj;
        if(remember_items.ContainsKey(place_name))
        {
            remember_items[place_name].Add(tmp);
        }
        else
        { 
            remember_items[place_name] = new List<memory_thing>() { tmp };
        }
    }

    //Returns true if the given thing  is in the given position in memory.
    public bool KnowThing(string place_name, Vector2Int pos, Thing obj)
    {
        if (!remember_items.ContainsKey(place_name))
            return false;
        else
        {
            foreach(memory_thing x in remember_items[place_name])
            {
                if (x.thing == obj && x.pos_at_place == pos)
                    return true;
            }
        }
        return false;
    }

    //Removes the given thing from the associated position in memory. Returns true if it succeeded.
    public bool RemoveThing(string place_name, Vector2Int pos, Thing obj)
    {
        if (!remember_items.ContainsKey(place_name))
            return false;
        else
        {
            foreach (memory_thing x in remember_items[place_name])
            {
                if (x.thing == obj && x.pos_at_place == pos)
                {
                    remember_items[place_name].Remove(x);
                    return true;
                }
            }
        }
        return false;
    }

    //Removes all things at the given position. Returns true if things were removed.
    public bool RemoveAllThings(string place_name, Vector2Int pos)
    {
        if (!remember_items.ContainsKey(place_name))
            return false;
        else
        {
            bool removed = false;
            List<memory_thing> rmv = new List<memory_thing>();
            foreach (memory_thing x in remember_items[place_name])
            {
                if (x.pos_at_place == pos)
                {
                    removed = true;
                    rmv.Add(x);
                }
            }

            foreach(memory_thing remove in rmv)
            {
                remember_items[place_name].Remove(remove);
            }

            return removed;
        }
    }

    //Removes all things at the given position. Returns true if things were removed.
    public bool RemoveAllPeople(string place_name, Vector2Int pos)
    {
        if (!remember_people.ContainsKey(place_name))
            return false;
        else
        {
            bool removed = false;
            List<memory_person> rmv = new List<memory_person>();
            foreach (memory_person x in remember_people[place_name])
            {
                if (x.pos_at_place == pos)
                {
                    removed = true;
                    rmv.Add(x);
                }
            }

            foreach (memory_person remove in rmv)
            {
                remember_people[place_name].Remove(remove);
            }

            return removed;
        }
    }
}
