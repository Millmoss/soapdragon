using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Blackboard {
    
    public static given_action GetNextMove(Person prsn)
    {
        List<given_action> potential_actions = new List<given_action>();

        given_action hngr = new given_action();
        hngr.value = HungerExpert(prsn);
        hngr.action = Enums.actions.hunger;
        potential_actions.Add(hngr);

        given_action stress = new given_action();
        stress.value = StressExpert(prsn);
        stress.action = Enums.actions.stress;
        potential_actions.Add(stress);

        given_action thirst = new given_action();
        thirst.value = ThirstExpert(prsn);
        thirst.action = Enums.actions.thirst;
        potential_actions.Add(thirst);

        given_action tired = new given_action();
        tired.value = TirednessExpert(prsn);
        tired.action = Enums.actions.tired;
        potential_actions.Add(tired);

        given_action libido = new given_action();
        libido.value = LibidoExpert(prsn);
        libido.action = Enums.actions.libido;
        potential_actions.Add(libido);


        //sort with reference to the value.
        potential_actions.Sort((s1, s2) => s2.value.CompareTo(s1.value));

        
        //TODO: return most important value based on logic. Right now just returns highest vlaue.

        return potential_actions[0];
        
    }

    private static float StressExpert(Person prsn)
    {
        float ret = 0;

        ret = Mathf.Clamp(prsn.GetNeedValue("stress") + 0.35f, 0, 1.20f);
        return ret;
    }

    private static float HungerExpert(Person prsn)
    {
        float ret = 0;

        ret = Mathf.Clamp(prsn.GetNeedValue("hunger")+0.35f,0,1.25f); 
        return ret;
    }

    private static float ThirstExpert(Person prsn)
    {
        float ret = 0;

        ret = Mathf.Clamp(prsn.GetNeedValue("thirst") + 0.35f, 0, 1.3f);
        return ret;
    }

    private static float TirednessExpert(Person prsn)
    {
        float ret = 0;

        ret = Mathf.Clamp(prsn.GetNeedValue("tiredness") + 0.35f, 0, 1.25f);
        return ret;
    }

    private static float LibidoExpert(Person prsn)
    {
        float ret = 0;

        ret = Mathf.Clamp(prsn.GetNeedValue("stress") + 0.35f, 0, 1.15f);
        ret = prsn.GetNeedValue("libido");
        return ret;
    }
}
