using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FarmSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmPlot;
    [SerializeField] private GameObject tree;

    [SerializeField] private int farmDimensions = 10;

    [SerializeField] private Color green;
    [SerializeField] private Color brown;

    public static Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawnFarm();
    }

    void spawnFarm()
    {
        for (int i = -1 * farmDimensions; i < farmDimensions; i++)
        {
            
            for (int j = -1 * farmDimensions; j < farmDimensions; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 1);
                GameObject spawned = Instantiate(farmPlot, transform);
                
                
                spawned.transform.position = pos;
                Vector2Int loc = (Vector2Int)pos;
                tiles.Add(loc, spawned);
                spawned.name = $"FarmTile {loc}";
                SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
                bool i_odd = math.abs(i) % 2 != 0;
                bool j_odd = math.abs(j) % 2 != 0;
                //a
                if (i_odd == j_odd)
                {
                    sr.color = green;
                }
                //b
                if (i_odd != j_odd)
                {
                    sr.color = green;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
