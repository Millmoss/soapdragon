using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory {

    //What rooms are associated with what actions. Change this to Room when implemented. 
    public Dictionary<Main,List<given_action.actions>> locations;

    //What items are associated with what actions.
    public Dictionary<string, List<given_action.actions>> items;

    //Where we think people were last. Change this to Room when implemented.
    public Dictionary<string, Main> people;

}
