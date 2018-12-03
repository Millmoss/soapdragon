using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* put down like a dog.
public class Item {
    
    public string name;
    private float val;
    private given_action.actions use;
    private int interact_time, init_interact_time;

    public Item(string _name, given_action.actions _type, float _val, int _interact_time)
    {
        name = _name;
        use = _type;
        val = _val;
        interact_time = _interact_time;
        init_interact_time = interact_time;
    }

    public given_action.actions GetUse()
    {
        return use;
    }

    public bool ObjectDone()
    {
        return interact_time == 0;
    }

    //If person is taken away from object, do things dependent on what type it is.
    public void ObjectReset()
    {
        //Food
        if(use == given_action.actions.eat)
        {

        }
        else
        {
            interact_time = init_interact_time;
        }
    }

    //Object stores it's own interact time
    public void InteractWith()
    {
        interact_time--;
    }

    //Overal value if done to completion.
    public float GetTotalVal()
    {
        return val;
    }

    //Per step
    public float GetStepVal()
    {
        return val / init_interact_time;
    }

}
*/