using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public PrintText txt;
    public Vector2Int boundries;
    Conversation c;

    Place cur_room = new Place(new Rectangle(new Vector2Int(0, 0), 5, 5, 0),"Home");

    private void Start()
    {
        c = new Conversation();
        List<Person> lp = FileManager.initPersons(cur_room);
        foreach(Person p in lp)
        {
            p.c = c;
            cur_room.AddPeople(p.Position, new List<Person>() { p });
        }

        List<Thing> thngs = new List<Thing>();
        thngs.Add(new Thing("Ice Cream", new Vector2Int(0, 5), 200, 2, 1, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        List<Thing> bing = new List<Thing>();
        bing.Add(new Thing("Cheesecake", new Vector2Int(0, -5), 200, 2, 1, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        List<Thing> ying = new List<Thing>();
        ying.Add(new Thing("Apple", new Vector2Int(0, -5), 200, 2, 1, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));


        List<Thing> ding = new List<Thing>();
        ding.Add(new Thing("Fruitcake", new Vector2Int(0, -5), 200, 2, 1, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        cur_room.AddThings(new Vector2Int(0, 5), thngs);
        cur_room.AddThings(new Vector2Int(0, -5), bing);
        cur_room.AddThings(new Vector2Int(-5, 0), ying);
        cur_room.AddThings(new Vector2Int(5, 0), ding);

        c = new Conversation();
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
        if(Input.GetKeyDown(KeyCode.P))
        {
            List<Vector2Int> x = Pathfinding.GetPath(new Vector2Int(2, 2),
                new Vector2Int(4, 5 ),
                new List<Vector2Int>() { new Vector2Int(3, 5), new Vector2Int(4,4), new Vector2Int(4,6) },
                new Vector2Int(4,4));

            foreach(Vector2Int p in x)
            {
                Debug.Log(p);
            }

        }
    }


}
