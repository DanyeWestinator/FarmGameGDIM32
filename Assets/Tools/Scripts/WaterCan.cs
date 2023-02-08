using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCan : Tool
{
    public override void Use(GameObject tile)
    {
        Plant plant = tile.GetComponentInChildren<Plant>();
        if (plant)
            plant.Water();
    }
}
