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

        ppl.Add(new Vector2Int(0,0), new Person("Bill", Person.gndr.Male, new Vector2Int(0,0),this));
        //ppl.Add(new Vector2Int(2,2), new Person("Janet", Person.gndr.Female));

        objects.Add(new Vector2Int(0, 5), new Item("Ice Cream", given_action.actions.eat, 0.5f,2));

    }

    public void RemoveItem(Vector2Int pos)
    {
        objects.Remove(pos);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(txt.IsTextComplete())
            { 
                print("D");
                foreach(Person x in ppl.Values)
                {
                    x.Update();
                    print(x.GetHunger());
                }
                foreach (Person x in ppl.Values)
                {
                    txt.SetText(x.Action(ppl,objects));
                }
            }
            else
            {
                txt.SetText("");
            }
        }
    }


}
