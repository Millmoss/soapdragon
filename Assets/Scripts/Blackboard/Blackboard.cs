﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Blackboard {
    
    public static given_action GetNextMove(Person prsn, Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        List<given_action> potential_actions = new List<given_action>();

        given_action hngr = new given_action();
        hngr.value = HungerExpert(prsn, ppl,itms);
        hngr.action = given_action.actions.eat;
        potential_actions.Add(hngr);

        given_action stress = new given_action();
        stress.value = StressExpert(prsn, ppl, itms);
        stress.action = given_action.actions.stress;
        potential_actions.Add(stress);


        //sort with reference to the value.
        potential_actions.Sort((s1, s2) => s2.value.CompareTo(s1.value));

        
        //TODO: return most important value based on logic. Right now just returns highest vlaue.

        return potential_actions[0];
        
    }

    private static float StressExpert(Person prsn, Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        float ret = 0;
        
        ret = prsn.GetStress();

        return ret;

    }

    private static float HungerExpert(Person prsn, Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        float ret = 0;

        ret = prsn.GetHunger();
        return ret;
    }

}
