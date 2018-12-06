using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Pathfinding  {

    public static Vector2Int GetPath(Vector2Int initPos, Vector2Int endPos, List<Vector2Int> cannotPass, Vector2Int boundries)
    {
        //Dikstras
        List<Vector2Int> open_list = new List<Vector2Int>();
        List<Vector2Int> closed_list = new List<Vector2Int>();
        open_list.Add(initPos);
        Vector2Int cur_pos = initPos;
        while(true)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            neighbors.Add(cur_pos + new Vector2Int(0, -1));
            neighbors.Add(cur_pos + new Vector2Int(0, 1));
            neighbors.Add(cur_pos + new Vector2Int(1, 0));
            neighbors.Add(cur_pos + new Vector2Int(-1, 0));

            closed_list.Add(cur_pos);

            foreach(Vector2Int v in neighbors)
            {

            }

            break;
        }


        return new Vector2Int(0, 0);
    }

}
