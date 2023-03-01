using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Tool
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(GameObject tile)
    {
        Plant plant = tile.GetComponentInChildren<Plant>();
        if (plant && plant.IsDead)
        {
            plant.Dig();
        }
    }
}
