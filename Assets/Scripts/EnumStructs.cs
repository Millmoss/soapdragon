using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums  {
    public enum uses		{ food, tired, thirst, libido, stress}
    public enum constraints { made_from_human_flesh }

    public enum actions { nothing, hunger, stress, thirst, tired, libido};
    public enum rotations { N, W, S, E };

	public enum gender		{ male, female, nonbinary };
    public enum color		{ red, blue, green, brown, white, black, pink };
	public enum descriptors { hostile, friendly, helpful, harmful, dangerous, safe, agreeable, disagreeable, loving }    //there will be a LOT of these, keeping it low for now

	public enum lineTypes	{ opinionDirected, opinionUndirectedPerson, opinionUndirectedOther, requestDirected, insultDirected, threatDirected, opinionOpinionSimple, questionReason, answerRequest, answerDanger, greeting }
	public enum expressionTypes	{ feelingVerb, feelingAdjective, amountAdverb, agreementVerb }
	public enum generalTypes { thing, person, place, feature, line }

	//Checks all related actions to their uses they want; e.g. eat wants food.
	public static Dictionary<actions,List<uses>> action_to_uses = new Dictionary<actions, List<uses>>() {
        {
            actions.hunger, new List<uses>(){uses.food}
        },
        {
            actions.tired, new List<uses>(){uses.tired}
        },
        {
            actions.stress, new List<uses>(){uses.stress}
        },
        {
            actions.libido, new List<uses>(){uses.libido}
        },
        {
            actions.thirst, new List<uses>(){uses.thirst}
        }
    };

    public static bool IsInDictionary(HashSet<uses> givn, actions used_action)
    {
        foreach(uses x in givn)
        {
            if(action_to_uses.ContainsKey(used_action))
                if (action_to_uses[used_action].Contains(x))
                    return true;
        }

        return false;
    }

    public static int GetRotation(Enums.rotations r)
    {
        if (r == Enums.rotations.E)
        {
            return 270;
        }
        else if (r == Enums.rotations.N)
            return 0;
        else if (r == Enums.rotations.W)
            return 90;
        else return 180;
    }
}

public struct memory_person
{
    public Person person;
    public Vector2Int pos_at_place;
}

public struct memory_thing
{
    public Thing thing;
    public Vector2Int pos_at_place;
}

public struct Preference_Things
{
    public float like_value;
    public Thing thing;
}

public struct Preference_People
{
    public float like_value;
    public Person person;
}

public struct given_action
{
    public Enums.actions action;
    public float value;
}
