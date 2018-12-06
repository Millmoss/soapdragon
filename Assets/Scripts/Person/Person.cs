using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {

	public string name { get; private set; }
	private Vector2Int position;
	public Memory memory { get; private set; }
	public Place current_place { get; private set; }

	//Emotions
	public float happiness { get; private set; }
	public float stress { get; private set; }
	//Mental
	public float wit { get; private set; }
	public float chill{ get; private set; }
	public float introversion { get; private set; }
    //Physical
    public string gender { get; private set; }
	public string hair { get; private set; }
	public string eyes { get; private set; }
    public float muscle { get; private set; }
	public float fat { get; private set; }
	public float beauty { get; private set; }
    public int eyesight { get; private set; }
	//height is in meters, weight is in lbs
	public float height { get; private set; }
	public float weight { get; private set; }
	//Feeling; -1 for hate, 1 for love.
	private List<Preference> likes;
	//Motivators
	public float hunger { get; private set; }
	public float social { get; private set; }

	//These are for pathfinding / AI interaction stuff.
	private Thing wanted_object;
	private Vector2Int moveto_position;
	private given_action current_action;

    private static Thing null_object = new Thing("NOTHING", Vector2Int.zero, 0, 0, 0, 0, 0, null, null, null);



    public Person(string _name, Enums.gender _gender, Vector2Int _pos, Place _current_room)
    {
        name = _name;
        happiness = 1;
        stress = 0;
        wit = 0.5f;
        chill = 0.5f;
        introversion = 0.5f;

        gender = _gender.ToString();
        hair = Enums.color.red.ToString();
        eyes = Enums.color.red.ToString();
        eyesight = 5;

        muscle = 0.5f;
        fat = 0.5f;
        height = 2.75f;
        weight = 175f;
        beauty = -1;

        likes = new List<Preference>();

        hunger = 0;
        social = 0;

        position = _pos;

        memory = new Memory();
        current_place = _current_room;

        wanted_object = null;

    }

    //Each step this function is called; live "updates"
    public void Update(Dictionary<Vector2Int, List<Person>> ppl, Dictionary<Vector2Int, List<Thing>> thngs)
    {
        Vector2Int checkPos = new Vector2Int();
        //Check everything in eyesight range, if you find something add it to memory.
        for (int y = 0; y < eyesight; y++)
        {
            checkPos.y = y + 1;
            for (int x = -y; x <= y; x++)
            {
                checkPos.x = x;
                //First we remove all memory of thigns @ the chcked positions.
                memory.RemoveAllThings(current_place.name, checkPos);

                //Now we go through and readd everything to to the memory using the new knowledge we have.
                if (thngs.ContainsKey(checkPos))
                {
                    foreach (Thing obj in thngs[checkPos])
                    {
                        memory.AddThing(current_place.name, checkPos, obj);
                    }
                }
            }
        }




        //Tick hunger / social stat.
        if (hunger < 1)
        {
            hunger += 0.1f;
        }

        if (social < 1)
        {
            social += (1.5f - introversion) * 0.1f;
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



    private string PerformSpeaking()
    {
        string ret = "";

        //He wants to talk.
        if (social > 0.75f)
        {
            if (likes.Count > 0)
            {
                //Write algorithmf or this. What do ppl talk about?
                int randNum = UnityEngine.Random.Range(0, likes.Count);
                //Noone is around.
                ret += name + " wants to talk about ";
                ret += likes[randNum].thing.name;
                ret += " with a like value of ";
                ret += likes[randNum].like_value;
                ret += ".";

                //Case for if someone is around.
            }
            else
            {
                ret = name + " wants to learn how to have opinions.";
            }
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

        if (wanted_object == null)
        {
            bool found = false;
            //If we remmeber there is a item @ position, we set that as the item we want to get to to interact with.
            if(memory.remember_items.ContainsKey(current_place.name))
            {
                foreach (memory_thing item in memory.remember_items[current_place.name])
                {
                    if (Enums.IsInDictionary(item.thing.uses, current_action.action))
                    {
                        found = true;
                        wanted_object = item.thing;
                        moveto_position = item.pos_at_place;
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
                        if (hunger < 1)
                        {
                            //TODO: Add handling when duravility is 0.
                            wanted_object.Damage();
                            hunger = Mathf.Clamp(hunger + wanted_object.nutrition, 0, 1);

                            return name + " is doing action " + next_action.action + " with original intention of " + next_action.value + " onto " + wanted_object.name +
                                " and his new hunger value is " + hunger + ".";
                        }
                        else
                        {
                            return name +"'s item is out of durabililty.";
                        }
                    }
                }
                else
                {
                    return FinishedAction();
                }

            }
        }
        else
        {
            return name + " has no idea what to do!";
        }

        //Move towards item; use actual pathfinding at some point.
        position.y++;


        return name + " is moving towards the " + wanted_object.name + " at position " + moveto_position +
            " with action " + current_action.action + " with intention " + current_action.value +
            "\nCurrently " + name + " is at now at position " + position;


    }

    //finished action    
    private string FinishedAction()
    {
        //Finished doing the stuff, apply changes.
        string ret = "";
        ret = name + " is finished with action " + current_action.action + " and is ready to do something else, with a hunger value of " + hunger + ".";

        current_action.action = Enums.actions.nothing;

        wanted_object = null;

        return ret;

    }

    //Is there a stored like value?
    private bool HasLike(string x)
    {
        for (int i = 0; i < likes.Count; i++)
        {
            if (likes[i].thing.name == x)
                return true;
        }
        return false;
    }

    //Adds items rn, prob add other thing later.
    public void AddLike(Thing itm)
    {
        if (!HasLike(itm.name))
        {
            Preference p = new Preference();
            p.thing = itm;

            //Write algorithm based on values for this. How do you define things like this?
            p.like_value = 0.75f;

            likes.Add(p);
        }
    }

	public Vector2Int Position
	{
		get { return new Vector2Int(position.x, position.y); }
	}

	public List<Preference> Likes
	{
		get { return new List<Preference>(likes); }
	}
}
