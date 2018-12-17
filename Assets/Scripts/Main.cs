using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Main : MonoBehaviour {

    public PrintText txt;
    public Vector2Int boundries;
    public Visual_Controller vc;
    List<Person> lp;
    Conversation c;
    public Tilemap floor;
    public Tile def, vision;

    Place cur_room = new Place(new Rectangle(new Vector2Int(0, 0), 5, 5, 0),"Home");

    private void Start()
    {
        c = new Conversation();
        lp = FileManager.initPersons(cur_room);
        foreach (Person p in lp)
        {
            p.tm = floor;
            p.t = vision;
            p.def = def;
            p.c = c;
            cur_room.AddPeople(p.Position, new List<Person>() { p });
            vc.AddPerson(p);
        }

        List<Thing> thngs = new List<Thing>();
        thngs.Add(new Thing("Ice Cream Maker", new Vector2Int(0, 5), 200, 2, -0.15f, 2f, 3,
            new HashSet<Enums.uses>() { Enums.uses.food }, new HashSet<Enums.constraints>()
            { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "bold", "brash", "belongs", "trash" }
            ));

        thngs.Add(new Thing("Coffee Injector", new Vector2Int(3, 6), 300, 5, -0.15f, -0.35f, 1,
            new HashSet<Enums.uses>() { Enums.uses.tired }, new HashSet<Enums.constraints>()
            { Enums.constraints.made_from_human_flesh },
            new HashSet<string>() { "strange", "drugs", "injection", "illegal" }
            ));

        thngs.Add(new Thing("Bottle of infinite water", new Vector2Int(-3, -2), 10, 9999, -0.05f, -0.15f, 1,
     new HashSet<Enums.uses>() { Enums.uses.thirst }, new HashSet<Enums.constraints>()
     { Enums.constraints.made_from_human_flesh },
     new HashSet<string>() { "liquid","water" }
     ));

        thngs.Add(new Thing("stress ball", new Vector2Int(3, -3), 10, 10, -0.225f, -0.185f, 1,
     new HashSet<Enums.uses>() { Enums.uses.stress }, new HashSet<Enums.constraints>()
     { Enums.constraints.made_from_human_flesh },
     new HashSet<string>() { "ball" }
     ));

        thngs.Add(new Thing("dont bring this to school kids", new Vector2Int(0, -7), 2000, 1, 
            -0.05f, -0.99f, 1,
     new HashSet<Enums.uses>() { Enums.uses.libido }, new HashSet<Enums.constraints>()
     { Enums.constraints.made_from_human_flesh },
     new HashSet<string>() { "not_very_sexy" }
     ));
        for (int i = 0;i<thngs.Count;i++)
        {
            cur_room.AddThing(thngs[i]);
            vc.AddThing(thngs[i]);
        }
        cur_room.PeopleUpdate();

        c = new Conversation();
    }
    
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            if(txt.IsTextComplete())
            {
                txt.SetText(cur_room.PeopleActivate());
                cur_room.PeopleUpdate();
                vc.UpdateTilemap();
            }
            else
            {
                txt.SetText("");
            }
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            foreach(Person p in lp)
            {
                Debug.Log(lp[0].memory.GetRandomPerson(cur_room).name);
            }
        }
    }


}
