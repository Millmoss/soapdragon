using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Visual_Controller : MonoBehaviour {

    public Tilemap tm_ppl, tm_things;
    public Tile prsna, prsnb, prsnc, thng;
    public List<Vector3Int> prev_pos;
    public List<Person> ppl;
    public List<Thing> thngs;

	// Use this for initialization
	void Awake () {
        prev_pos = new List<Vector3Int>();
        ppl = new List<Person>();
        thngs = new List<Thing>();
	}
	
    public void AddPerson(Person p)
    {
        Vector3Int pos = new Vector3Int(p.Position.x, p.Position.y,0);
		if (ppl.Count == 0)
		{
			tm_ppl.SetTile(pos, prsna);
			print(p.name + "reD");
		}
		if (ppl.Count == 1)
		{
			tm_ppl.SetTile(pos, prsnb);
			print(p.name + "bkl");
		}
		if (ppl.Count == 2)
		{
			tm_ppl.SetTile(pos, prsnc);
			print(p.name + "gre");
		}

		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero,
            Quaternion.Euler(0,0,Enums.GetRotation(p.rotation)), 
            Vector3.one);
        tm_ppl.SetTransformMatrix(pos, m);

        prev_pos.Add(pos);
        ppl.Add(p);
    }

    public void AddThing(Thing t)
    {
        Vector3Int pos = new Vector3Int(t.position.x, t.position.y, 0);
        tm_things.SetTile(pos, thng);
        prev_pos.Add(pos);
        thngs.Add(t);
    }

	// Update is called once per frame
	public void UpdateTilemap () {
		for(int i = 0;i < prev_pos.Count;i++)
        {
            tm_ppl.SetTile(prev_pos[i], null);
            tm_things.SetTile(prev_pos[i], null);
        }
        for (int i = 0; i < ppl.Count; i++)
        {
            Vector3Int pos = new Vector3Int(ppl[i].Position.x, ppl[i].Position.y, 0);
			if (i == 0)
				tm_ppl.SetTile(pos, prsna);
			if (i == 1)
				tm_ppl.SetTile(pos, prsnb);
			if (i == 2)
				tm_ppl.SetTile(pos, prsnc);
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero,
                Quaternion.Euler(0, 0, Enums.GetRotation(ppl[i].rotation)),
                Vector3.one);
            tm_ppl.SetTransformMatrix(pos, m);
            prev_pos.Add(pos);
        }
        for(int i = 0;i < thngs.Count;i++)
        {
            Vector3Int pos = new Vector3Int(thngs[i].position.x, thngs[i].position.y, 0);
            tm_things.SetTile(pos, thng);
            prev_pos.Add(pos);
        }
    }
}
