using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static T GetRandom<T>(this List<T> list)
    {
        //Random.range is exclusive on the upper bound
        return list[Random.Range(0, list.Count)];
    }

    public static Vector2Int tileFromGO(this GameObject go)
    {
        Vector2Int pos = Vector2Int.RoundToInt(go.transform.position);
        return pos;
    }
}
