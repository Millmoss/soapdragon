using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {

    private string name;
    private Vector2Int position;
    private Memory memory;

    //Enums
    public enum gndr { Male, Female, Nonbinary};
    public enum clr { red, blue, green, brown, white, black, pink};

    //Emotions
    private float happiness, stress;
    //Mental
    private float wit, chill, introversion;
    //Physical
    private gndr gender;
    private clr hair, eyes;
    private float muscle, fat, beauty;
    //height is in meters, weight is in lbs
    private float height, weight;
    //Feeling; -1 for hate, 1 for love.
    private List<Preference> likes;
    //Motivators
    private float hunger, social;

    //These are for pathfinding / AI interaction stuff.
    private Item wanted_object;
    private Vector2Int moveto_position;
    private given_action current_action;

    private static Item null_object = new Item("NOTHING", given_action.actions.nothing, 0, 0);


    //Change this to Room when rooms are implemented.
    private Main current_room;

 
    

    public float GetStress()
    {
        return stress;
    }

    public float GetHunger()
    {
        return hunger;
    }

    //Replace Main with Room when Room is implemented.
    public Person(string _name, gndr _gender, Vector2Int _pos,Main _current_room)
    {
        name = _name;
        happiness = 1;
        stress = 0;
        wit = 0.5f;
        chill = 0.5f;
        introversion = 0.5f;

        gender = _gender;
        hair = clr.red;
        eyes = clr.red;

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
        current_room = _current_room;

        wanted_object = null;

    }

    public void Update()
    {
        //rn this increases hunger by 0.1.
        if(hunger<1)
        {
            hunger += 0.1f;
        }

        if(social < 1)
        {
            social += (1.5f - introversion) * 0.1f; 
        }
    }

    public string Action(Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        string ret = "";
        ret += PerformAction(ppl, itms);
        string ps = PerformSpeaking(ppl, itms);
        if(ps != "")
        { 
            ret += "\n";
            ret += ps;
        }
        return ret;

    }



    private string PerformSpeaking(Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        string ret = "";

        //He wants to talk.
        if(social > 0.75f)
        { 
            if(likes.Count > 0)
            {
                //Write algorithmf or this. What do ppl talk about?
                int randNum = UnityEngine.Random.Range(0, likes.Count);
                //Noone is around.
                ret += name + " wants to talk about ";
                ret += likes[randNum].item.name;
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

    private string PerformAction(Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        given_action next_action = Blackboard.GetNextMove(this, ppl, itms);
        if(current_action.action!=next_action.action)
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
            //If there's an item @ position, do thing.
            foreach (Vector2Int item_pos in itms.Keys)
            {
                if(itms[item_pos].GetUse() == next_action.action)
                {
                    found = true;
                    wanted_object = itms[item_pos];
                    moveto_position = item_pos;
                    break;
                }
            }
            if(!found)
            {
                wanted_object = null_object;
            }
        }

        if (wanted_object != null_object)
        { 

            //IF we're at the position, do the stuff.
            if(position == moveto_position)
            {
                //How long does it take to do the stuff?
                current_room.RemoveItem(moveto_position);


                if (!wanted_object.ObjectDone())
                {
                    //Apply the change in value at every step.
                    switch (wanted_object.GetUse())
                    {
                        case given_action.actions.eat:
                            hunger = Mathf.Clamp(hunger - wanted_object.GetStepVal(), 0, 1);
                            break;

                        default:
                            break;
                    }

                    //Because you're currently itneracting wiht it, add it to the likes if theres no opinion. 
                    AddLike(wanted_object);

                    //Tick the time up after you do it.
                    wanted_object.InteractWith();
                    return name + " is doing action " + next_action.action + " with original intention of " + next_action.value + " onto "+wanted_object.name+
                        " and his new hunger value is "+hunger+".";
                }
                else
                {
                    //Finished doing the stuff, apply changes.

                    current_action.action = given_action.actions.nothing;

                    //Add opinion. This wil


                    wanted_object = null;

                    return name + " is finished with action " + next_action.action + " and is ready to do something else, with a hunger value of " + hunger+".";
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
            "\nCurrently "+name+" is at now at position " + position;
        
        
    }

    //Is there a stored like value?
    private bool HasLike(string x)
    {
        for(int i= 0;i < likes.Count;i++)
        {
            if (likes[i].item.name == x)
                return true;
        }
        return false;
    }

    //Adds items rn, prob add other thing later.
    public void AddLike(Item itm)
    {
        if (!HasLike(itm.name))
        {
            Preference p = new Preference();
            p.item = itm;

            //Write algorithm based on values for this. How do you define things like this?
            p.like_value = 0.75f;

            likes.Add(p);
		}
	}

}

//Make this support people as well. Thinking of just splitting preferences up to people / items for clarity / ease of use.
public struct Preference
{
    public float like_value;
    public Item item;
}

public struct given_action
{
    public enum actions { nothing, move, eat, stress };
    public actions action;
    public float value;
}
