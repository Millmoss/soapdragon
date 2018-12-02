using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public enum typ { food }
    public string name;
    private float val;
    private typ type;

    public Item(string _name, typ _type, float val)
    {
        name = "Ice cream";
        type = typ.food;
        val = 0.5f;
    }

}
