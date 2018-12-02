using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMap : MonoBehaviour
{

    public Unit[] red_units;
    public Unit[] blue_units;
    public Vector2Int size;
    public Tilemap tm;
    public Tile red_tile, blue_tile, neutral_tile, red_edge,blue_edge,red_unit,blue_unit;

    private internal_tile[,] map;

    // Use this for initialization
    void Start()
    {
        InstantiateMap();

     
    }

    void InstantiateMap()
    {
        map = new internal_tile[size.x, size.y];

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                map[i, j] = new internal_tile();
            }
        }

        for (int i = 0; i < red_units.Length; i++)
        {
            map[red_units[i].position.x, red_units[i].position.y].influence = red_units[i].influence;
            map[red_units[i].position.x, red_units[i].position.y].setTeam("red");
        }
        for (int i = 0; i < blue_units.Length; i++)
        {
            map[blue_units[i].position.x, red_units[i].position.y].influence = blue_units[i].influence;
            map[blue_units[i].position.x, red_units[i].position.y].setTeam("blue");
        }
        UpdateMap();
    }

    void UpdateMap()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (map[i, j].getTeam() == "red")
                    tm.SetTile(new Vector3Int(i, j, 0), red_tile);
                else if (map[i, j].getTeam() == "blue")
                    tm.SetTile(new Vector3Int(i, j, 0), blue_tile);
                else
                    tm.SetTile(new Vector3Int(i, j, 0), neutral_tile);
            }
        }

    }

    void Step()
    {
        for(int i=0;i < red_units.Length;i++)
        {
            int infl = red_units[i].influence;
            Vector2Int pos = red_units[i].position;
            int y = infl;
            while(y > 0)
            { 
                //Top and bottom
                for(int x = 0; x < infl - y;x++)
                {
                    //fix later
                    if(pos.x + x <= size.x)
                    { 
                    tm.SetTile(new Vector3Int(pos.x + x, pos.y + y, 0), red_edge);
                    tm.SetTile(new Vector3Int(pos.x - x, pos.y - y, 0), red_edge);
                    map[pos.x + x, pos.y + y].influence = y ;
                    print(map[pos.x + x, pos.y + y].influence);
                    }
                }
                for (int x = -1; x > -infl + y; x--)
                {
                    tm.SetTile(new Vector3Int(pos.x + x, pos.y + y, 0), red_edge);
                    tm.SetTile(new Vector3Int(pos.x - x, pos.y - y, 0), red_edge);
                }
                y--;
                //side
                if (y != 0)
                { 
                    tm.SetTile(new Vector3Int(pos.x + y, pos.y, 0), red_edge);
                    tm.SetTile(new Vector3Int(pos.x - y, pos.y, 0), red_edge);
                }

            }
        }
        UpdateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Step();
        }
    }
}
    [System.Serializable]
public class Unit
{
    public Vector2Int position;
    public int influence;
}

class internal_tile
{
    public int influence;
    public enum type { red, blue, none};
    public type team;

    public internal_tile()
    {
        influence = 0;
        team = type.none;
    }

    public void setTeam(string x)
    {
        team = (type)System.Enum.Parse(typeof(type), x);
    }

    public string getTeam()
    {
        return team.ToString();
    }
}

