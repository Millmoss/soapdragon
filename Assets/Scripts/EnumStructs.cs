using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums  {
    public enum uses		{ food }
    public enum constraints { made_from_human_flesh }
    public enum actions		{ nothing, move, eat, stress };
	
	public enum gender		{ Male, Female, Nonbinary };
    public enum color		{ red, blue, green, brown, white, black, pink };
	public enum descriptors { hostile, friendly, helpful, harmful, dangerous, safe }    //there will be a LOT of these, keeping it low for now
	public enum lineTypes	{ opinionDirected, opinionUndirected, requestDirected, insultDirected, threatDirected, opinionOpinionSimple, questionReason, answerRequest, answerDanger }
	public enum expressionTypes	{ feelingVerb, feelingAdjective, amountAdverb, agreementVerb }

	//Checks all related actions to their uses they want; e.g. eat wants food.
	public static Dictionary<actions,List<uses>> action_to_uses = new Dictionary<actions, List<uses>>() {
        {
            actions.eat, new List<uses>(){uses.food}
        }
    };

    public static bool IsInDictionary(HashSet<uses> givn, actions used_action)
    {
        foreach(uses x in givn)
        {
            if (action_to_uses[used_action].Contains(x))
                return true;
        }

        return false;
    }
}

public struct memory_person
{
    public Person thing;
    public Vector2Int pos_at_place;
}

public struct memory_thing
{
    public Thing thing;
    public Vector2Int pos_at_place;
}

public struct Preference
{
    public float like_value;
    public Thing thing;
}

public struct given_action
{
    public Enums.actions action;
    public float value;
}
