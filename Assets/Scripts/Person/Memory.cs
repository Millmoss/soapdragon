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

    public Memory()
    {
        locations_assoc = new Dictionary<Place, List<Enums.actions>>();
        items_assoc = new Dictionary<string, List<Enums.actions>>();
        remember_items = new Dictionary<string, List<memory_thing>>();
        remember_people = new Dictionary<string, List<memory_person>>();
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
}
