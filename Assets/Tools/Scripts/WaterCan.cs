using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCan : Tool
{
    public AudioSource canWatering;
    public override void Use(GameObject tile)
    {
        canWatering.Play();
        Plant plant = tile.GetComponentInChildren<Plant>();
        if (plant)
            plant.Water();
    }
}
