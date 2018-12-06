using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Uses A*Star pathfinding.
static class Pathfinding  {

    private static Vector2Int GetSmallest(Dictionary<Vector2Int,float> list)
    {
        float smallest_f = float.MaxValue;
        Vector2Int ret = Vector2Int.zero;

        foreach(Vector2Int x in list.Keys)
        {
            if(list[x] < smallest_f)
            {
                smallest_f = list[x];
                ret = x;
            }
        }

        return ret;

    }

    public static List<Vector2Int> GetPath(Vector2Int initPos, Vector2Int endPos, 
        List<Vector2Int> cannotPass, Vector2Int boundries)
    {
        Dictionary<Vector2Int,float> open_list = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> closed_list = new Dictionary<Vector2Int, float>();
        open_list.Add(initPos, 0);
  
        while(open_list.Count > 0)
        {
            Vector2Int cur_pos = GetSmallest(open_list);
            open_list.Remove(cur_pos);

            if (cur_pos == endPos)
            {
                List<Vector2Int> ret = new List<Vector2Int>();

                foreach (Vector2Int x in closed_list.Keys)
                    ret.Add(x);
                ret.Add(cur_pos);

                return ret;
            }

            closed_list.Add(cur_pos, Vector2Int.Distance(cur_pos, endPos));

            List<Vector2Int> neighbors = new List<Vector2Int>();
            neighbors.Add(cur_pos + new Vector2Int(0, -1));
            neighbors.Add(cur_pos + new Vector2Int(0, 1));
            neighbors.Add(cur_pos + new Vector2Int(1, 0));
            neighbors.Add(cur_pos + new Vector2Int(-1, 0));


            foreach (Vector2Int v in neighbors)
            {
                if (cannotPass.Contains(v))
                    continue;

                if (Mathf.Abs(v.x) > boundries.x || Mathf.Abs(v.y) > boundries.y)
                    continue;
                float cur_g = Vector2Int.Distance(v, endPos);
                //if neighbor is in closed list
                if(closed_list.ContainsKey(v))
                {
                    if(closed_list[v] > cur_g)
                    {
                        closed_list[v] = cur_g;
                    }
                }
                else if(open_list.ContainsKey(v))
                {
                    if(open_list[v] > cur_g)
                    {
                        open_list[v] = cur_g;
                    }
                }
                else
                {
                    open_list.Add(v, cur_g);
                }
            }
        }
        return new List<Vector2Int>();
        
    }

}
