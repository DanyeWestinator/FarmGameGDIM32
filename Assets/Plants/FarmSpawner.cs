using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FarmSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmPlot;

    [SerializeField] private int farmDimensions = 10;

    [SerializeField] private Color green;
    [SerializeField] private Color brown;
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
                Vector3 pos = new Vector3(i, j, 1);
                GameObject spawned = Instantiate(farmPlot, parent);
                spawned.name = $"FarmTile({pos})";
                spawned.transform.position = pos;
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
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
