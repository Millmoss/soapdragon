using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {

    //Range for GetRandomNearbyItem
    public static int random_radius = 6; 

	public string name { get; private set; }
    public int eyesight { get; private set; }
    private Vector2Int position;
    private Enums.rotations rotation;
	public Memory memory { get; private set; }
	public Place current_place { get; private set; }

    Dictionary<string, float> needs = new Dictionary<string, float>()
    {
        {"hunger", 0 },
        {"social", 0 },
        {"thrist",0 },
        {"tiredness",0 },
        {"stress", 0f },
        {"libido", 0f }
    };

    Dictionary<string,float> features_float = new Dictionary<string, float>() {
        //emotions
        {"happiness", 0.5f },
        //mental
        {"wit",  0.5f },
        {"chill", 0.5f },
        {"introversion",0.5f },
        //physical
        {"muscle",0.5f },
        {"fat",0.5f },
        {"beauty",0.5f },
        //weight is in meteres, weight in pounds.
        {"height",3 },
        {"weight", 125 },   
    };

    Dictionary<string, string> features_string = new Dictionary<string, string>()
    {
        {"gender","Male" },
        {"hair", "red" },
        {"eyes", "blue" }
    };
    //Feeling; -1 for hate, 1 for love.
    private List<Preference_Things> likes_things;
    private List<Preference_People> likes_people;

    //Pair values are Type -> Value -> how much is liked. Prob 2? One fo rstrings and one for floats 
    //Unless we can think of something not dumb.
    //EX: KeyValuePair<"gender","Male">, 0.1
    private Dictionary<KeyValuePair<string, string>, float> likes_traits;

    //These are for pathfinding / AI interaction stuff.
    private Thing wanted_object;
	private Vector2Int moveto_position;
	private given_action current_action;
    private List<Vector2Int> path_to_obj;

    private static Thing null_object = new Thing("NOTHING", Vector2Int.zero, 0, 0, 0, 0, 0, null, null, null);



    public Person(string _name, Enums.gender _gender, Vector2Int _pos, Place _current_room, 
        Dictionary<KeyValuePair<string, string>, float> _likes_traits,
		Dictionary<string, string> feat_str)
    {
        name = _name;
        eyesight = 5;

        likes_people = new List<Preference_People>();

        position = _pos;

        memory = new Memory();
        current_place = _current_room;

        rotation = Enums.rotations.W;

        wanted_object = null;
        likes_traits = _likes_traits;
		features_string = feat_str;
    }
    
	public void setChill(float c)
	{
		features_float["chill"] = c;
	}

    //Returns true if the person has the given trait.
    public bool HasFeature(string feature, string value)
    {
        if(features_string.ContainsKey(feature))
        {
            if (features_string[feature] == value)
                return true;
        }
        return false; 
    }

    //Returns true if a person's feature has the given value.
    public bool HasFeature(string feature, float value)
    {
        if (features_float.ContainsKey(feature))
        {
            //Has a flex area of 0.1f.
            if (0.1f >= Mathf.Abs(features_float[feature]- value))
                return true;
        }
        return false;
    }

    //Return closest trait thisperson has feeling towards person prsn towards w float feeling.
    //Always returns a value. 
    public string GetMatchingFeature(Person prsn, float feeling)
    {
        float dif = float.MaxValue;
        string feature = "";
        foreach(KeyValuePair<string, string> x in likes_traits.Keys)
        {
            if(Mathf.Abs(likes_traits[x] - feeling) < dif)
            {
                if (prsn.HasFeature(x.Key, x.Value))
                { 
                    dif = Mathf.Abs(likes_traits[x] - feeling);
                    feature = x.Key;
                }
            }
        }

        return feature;
    }

    //returns null if theres no items.
    public Thing GetRandomNearbyItem()
    {
        List<Thing> items = memory.GetThings(current_place.name);
        Thing ret = null;
        if (items.Count > 0)
            ret = items[UnityEngine.Random.Range(0, items.Count)];
        return ret;
    }

    //Each step this function is called; live "updates"
    public void Update(Dictionary<Vector2Int, List<Person>> ppl, Dictionary<Vector2Int, List<Thing>> thngs)
    {

        Vector2Int checkPos = new Vector2Int();
        //Check everything in eyesight range.
        int comp = 1;
        if (rotation == Enums.rotations.S)
            comp *= -1;
        for (int y = 0; y < eyesight; y++)
        {
            if (rotation == Enums.rotations.N || rotation == Enums.rotations.S)
                checkPos.y = y + 1;
            else
                checkPos.x = y + 1;
            checkPos.y *= comp;
            for (int x = -y; x <= y; x++)
            {
                if (rotation == Enums.rotations.N || rotation == Enums.rotations.S)
                    checkPos.x = x;
                else
                    checkPos.y = x;
                //First we remove all memory of thigns @ the chcked positions.
                memory.RemoveAllThings(current_place.name, checkPos);
                //We do the same with the people as well.
                memory.RemoveAllPeople(current_place.name, checkPos);

                //Now we go through and readd everything to to the memory using the new knowledge we have.
                if (thngs.ContainsKey(checkPos))
                {
                    foreach (Thing obj in thngs[checkPos])
                    {
                        memory.AddThing(current_place.name, checkPos, obj);
                    }
                }
                if (ppl.ContainsKey(checkPos))
                {
                    foreach (Person prsn in ppl[checkPos])
                    {
                        memory.AddPerson(current_place.name, checkPos, prsn);
                    }
                }
            }
        }




        //Tick hunger / social stat.
        if (needs["hunger"] < 1)
        {
            needs["hunger"] += 0.1f;
        }

        if (needs["social"] < 1)
        {
            needs["social"] += (1.5f - features_float["introversion"]) * 0.1f;
        }
    }

    //Perform actions based on memory.
    public string Action()
    {
        string ret = "";
        ret += PerformAction();
        string ps = PerformSpeaking();
        if (ps != "")
        {
            ret += "\n";
            ret += ps;
        }
        return ret;

    }

    //Returns the value of the given need.
    public float GetNeedValue(string x)
    {
        return needs[x];
    }

    //Returns the value of the given need.
    public float GetFeatureFloatValue(string x)
    {
        return features_float[x];
    }

    //Returns the value of the given need.
    public string GetFeatureStringValue(string x)
    {
        return features_string[x];
    }

    private string PerformSpeaking()
    {
        string ret = "";

        //He wants to talk.
        if (needs["social"] > 0.75f)
        {
            
        }
        return ret;
    }

    private string PerformAction()
    {
        given_action next_action = Blackboard.GetNextMove(this);
        if (current_action.action != next_action.action)
        {
            current_action = next_action;

            return name + " realized he needed to satisfy " + current_action.action + " with a value of " + next_action.value;
        }
        else
        {
            current_action.value = next_action.value;
        }
        //Basic movement algorithm
        if(wanted_object!=null)
            Debug.Log(wanted_object.name);
        if (wanted_object == null)
        {
            Debug.Log("Is null!");
            Debug.Log(memory.remember_items.Count);
            bool found = false;
            foreach(string x in memory.remember_items.Keys)
            {
                Debug.Log(x);
            }
            //If we remmeber there is a item @ position, we set that as the item we want to get to to interact with.
            if(memory.remember_items.ContainsKey(current_place.name))
            {
                foreach (memory_thing item in memory.remember_items[current_place.name])
                {
                    Debug.Log(item.thing.name);
                    if (Enums.IsInDictionary(item.thing.uses, current_action.action))
                    {
                        found = true;
                        wanted_object = item.thing;
                        moveto_position = item.pos_at_place;
                        path_to_obj = Pathfinding.GetPath(position, moveto_position,
                            new List<Vector2Int>(), new Vector2Int(999, 999));
                        break;
                    }
                }
            }
            if (!found)
            {
                wanted_object = null_object;
            }
        }

        if (wanted_object != null_object)
        {

            //IF we're at the position, do the stuff.
            if (position == moveto_position)
            {
                //How long does it take to do the stuff? Need this functionality.
                //current_room.RemoveItem(moveto_position);

                //REDO: TODO: What happens when bar is full?

                //Big switch case odds are? This is EAT
                if (wanted_object.durability > 0)
                {
                    if (current_action.action == Enums.actions.eat)
                    {
                        if (needs["hunger"] < 1)
                        {
                            //TODO: Add handling when duravility is 0.
                            wanted_object.Damage();
                            needs["hunger"] = Mathf.Clamp(needs["hunger"] + wanted_object.nutrition, 0, 1);

                            return name + " is doing action " + next_action.action + " with original intention of " + next_action.value + " onto " + wanted_object.name +
                                " and his new hunger value is " + needs["hunger"] + ".";
                        }
                        else
                        {
                            return FinishedAction();
                        }
                    }
                }
                else
                {
                    return name + "'s item is out of durabililty.";
                }

            }
        }
        else
        {
            return name + " has no idea what to do!";
        }

        //Move towards item; use actual pathfinding at some point.
        position = path_to_obj[0];
        path_to_obj.RemoveAt(0);


        return name + " is moving towards the " + wanted_object.name + " at position " + moveto_position +
            " with action " + current_action.action + " with intention " + current_action.value +
            "\nCurrently " + name + " is at now at position " + position;


    }

    //finished action    
    private string FinishedAction()
    {
        //Finished doing the stuff, apply changes.
        string ret = "";
        ret = name + " is finished with action " + current_action.action + 
            " and is ready to do something else, with a hunger value of " + needs["hunger"] + ".";

        current_action.action = Enums.actions.nothing;

        wanted_object = null;

        return ret;

    }

    //Is there a stored like value?
    private bool LikesItem(string x)
    {
        for (int i = 0; i < likes_things.Count; i++)
        {
            if (likes_things[i].thing.name == x)
                return true;
        }
        return false;
    }

    //Adds items rn, prob add other thing later.
    public void AddLikeItem(Thing itm)
    {
        if (!LikesItem(itm.name))
        {
            Preference_Things p = new Preference_Things();
            p.thing = itm;

            //Write algorithm based on values for this. How do you define things like this?
            p.like_value = 0.75f;

            likes_things.Add(p);
        }
    }

	public Vector2Int Position
	{
		get { return new Vector2Int(position.x, position.y); }
	}

	public List<Preference_Things> Likes
	{
		get { return new List<Preference_Things>(likes_things); }
	}
}
