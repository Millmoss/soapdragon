using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public PrintText txt;
    public Vector2Int boundries;

    Dictionary<Vector2Int, Person> ppl;
    Dictionary<Vector2Int, Item> objects;

    private void Start()
    {
        ppl = new Dictionary<Vector2Int, Person>();
        objects = new Dictionary<Vector2Int, Item>();

        ppl.Add(new Vector2Int(0,0), new Person("Bill", Person.gndr.Male));
        //ppl.Add(new Vector2Int(2,2), new Person("Janet", Person.gndr.Female));

        objects.Add(new Vector2Int(5, 5), new Item("Ice Cream",Item.typ.food,0.5f));

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Person x in ppl.Values)
            {
                x.Update();
            }
        }
    }


}
