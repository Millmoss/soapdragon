using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {

    //Enums
    public enum gndr { Male, Female, Nonbinary};
    public enum clr { red, blue, green, brown, white, black, pink};

    private string name;

    //Emotions
    private float happiness, stress;
    //Mental
    private float wit, chill;
    //Physical
    private gndr gender;
    private clr hair, eyes;
    private float muscle, fat, beauty;
        //height is in meters, weight is in lbs
    private float height, weight;
    //Feeling
    private Dictionary<string, float> preference;
    //Needs
    private float hunger;

    //Assumes that _gender fits the gndr enum.
    public Person(string _name, gndr _gender)
    {
        name = _name;
        happiness = 1;
        stress = 0;
        wit = 0.5f;
        chill = 0.5f;

        gender = _gender;
        hair = clr.red;
        eyes = clr.red;

        muscle = 0.5f;
        fat = 0.5f;
        height = 2.75f;
        weight = 175f;
        beauty = -1;

        preference = new Dictionary<string, float>();

        hunger = 0;

    }

    public void Update()
    {
        //rn this increases hunger by 0.1.
        if(hunger<1)
        {
            hunger += 0.1f;
        }
    }

    public given_action DoAction(Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        return Blackboard.GetNextMove(this, ppl, itms);
    }

}

public struct given_action
{
    public enum actions { move, eat };
    public actions action;
    public float value;
}
