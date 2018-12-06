using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public PrintText txt;
    public Vector2Int boundries;


    Place cur_room = new Place(new Rectangle(new Vector2Int(0, 0), 5, 5, 0),"Home");
    
    private void Start()
    {
        Dictionary<KeyValuePair<string,string>, float> trait_likes = new Dictionary<KeyValuePair<string,string>, float>()
        {
            {new KeyValuePair<string, string>("gender","Male"), 0.86f},
            {new KeyValuePair<string, string>("hair","red"), 0.86f}
        };

        List<Person> ppl= new List<Person>();
        ppl.Add(new Person("Bill", Enums.gender.Male, new Vector2Int(0, 0), cur_room, trait_likes));

        cur_room.AddPeople(new Vector2Int(0,0), ppl);

        //This should be instantiated via a file parser, so we don't have to do all this by had soon thank god.
        List<Thing> thngs = new List<Thing>();
        thngs.Add(new Thing("Ice Cream", new Vector2Int(0, 5), 200, 2, 0, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        List<Thing> bing = new List<Thing>();
        bing.Add(new Thing("Vice Meme", new Vector2Int(0, -5), 200, 2, 0, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        List<Thing> ying = new List<Thing>();
        ying.Add(new Thing("West is left", new Vector2Int(0, -5), 200, 2, 0, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));


        List<Thing> ding = new List<Thing>();
        ding.Add(new Thing("East is soggy", new Vector2Int(0, -5), 200, 2, 0, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>() { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        cur_room.AddThings(new Vector2Int(0, 5), thngs);
        cur_room.AddThings(new Vector2Int(0, -5), bing);
        cur_room.AddThings(new Vector2Int(-5, 0), ying);
        cur_room.AddThings(new Vector2Int(5, 0), ding);

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
