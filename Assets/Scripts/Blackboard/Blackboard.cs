using System.Collections;
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



        return new given_action();
    }

    private static float HungerExpert(Person prsn, Dictionary<Vector2Int, Person> ppl, Dictionary<Vector2Int, Item> itms)
    {
        float ret = 0;

        return ret;
    }

}
