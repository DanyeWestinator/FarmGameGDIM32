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

    public static Dictionary<Vector3Int, GameObject> tiles = new Dictionary<Vector3Int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawnFarm();
    }

    void spawnFarm()
    {
        for (int i = -1 * farmDimensions; i < farmDimensions; i++)
        {
            Transform parent = new GameObject().transform;
            parent.parent = transform;
            parent.position = Vector3.zero;
            parent.gameObject.name = $"X_val{i}";
            for (int j = -1 * farmDimensions; j < farmDimensions; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 1);
                GameObject spawned = Instantiate(farmPlot, parent);
                
                spawned.name = $"FarmTile({pos})";
                spawned.transform.position = pos;
                pos.z = 0;
                tiles.Add(pos, spawned);
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
                    sr.color = brown;
                }

                if (i == -2)
                {
                    GameObject treeSpawned = Instantiate(tree, spawned.transform);
                    spawned.GetComponent<Plant>().occupied = treeSpawned;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
