using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public PrintText txt;
    public Vector2Int boundries;


    Place cur_room = new Place(new Rectangle(new Vector2Int(0, 0), 5, 5, 0),"Home");
    
    private void Start()
    {

        List<Person> ppl= new List<Person>();
        ppl.Add(new Person("Bill", Enums.gender.Male, new Vector2Int(0, 0), cur_room));

        cur_room.AddPeople(new Vector2Int(0,0), ppl);

        //This should be instantiated via a file parser, so we don't have to do all this by had soon thank god.
        List<Thing> thngs = new List<Thing>();
        thngs.Add(new Thing("Ice Cream", new Vector2Int(0, 5), 200, 2, 0, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        cur_room.AddThings(new Vector2Int(0, 5), thngs);

    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            if(txt.IsTextComplete())
            {
                cur_room.PeopleUpdate();
                txt.SetText(cur_room.PeopleActivate());
            }
            else
            {
                txt.SetText("");
            }
        }
    }


}
