using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FarmSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmPlot;
    [SerializeField] private GameObject tree;

    [SerializeField] private int farmDimensions = 10;
    [SerializeField] private int treePercentage = 5;

    [SerializeField] private Color green;
    [SerializeField] private Color brown;

    /// <summary>
    /// All farm plots in game, key is their position in ints, value is tileGO
    /// </summary>
    public static Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawnFarm();
    }

    void spawnFarm()
    {
        tiles = new Dictionary<Vector2Int, GameObject>();
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

                int rand = Random.Range(1, 100);
                if (treePercentage >= rand)
                {
                    SpawnTree(spawned);
                }

            }
        }
    }

    void SpawnTree(GameObject farmPlot)
    {
        FarmTile plot = farmPlot.GetComponent<FarmTile>();
        GameObject tree = Instantiate(this.tree, farmPlot.transform);
        plot.occupiedBy = tree;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
