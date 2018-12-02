using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WorldMap : MonoBehaviour {

    //custom text parser for the map
    char[][] map;
    public Tilemap world;

    //custom parser to set dictionary for tiles.
    private Dictionary<char,Tile> tiles;

    public Tile tl;

	// Use this for initialization
	void Start () {
        map = new char[3][];
        for(int i=0;i<3;i++)
        {
            map[i] = new char[3];
        }

        //y
        for(int i=0;i<3;i++)
        {
            //x
            for (int j = 0; map[i].Length < 3; j++)
            {
                map[i][j] = 'C';
            }

        }

        ApplyMap();
	}

    //Sets the World tilemap's tiles according to map.
    public void ApplyMap()
    {
        //y
        for(int i=0;i < map.Length;i++)
        {
            //x
            for(int j=0;j<map[i].Length;j++)
            {
                world.SetTile(new Vector3Int(j, i, 0),tl);
            }
        }
    }
	
}
