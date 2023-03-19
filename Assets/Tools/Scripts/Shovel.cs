using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Tool
{
    public AudioSource shovelDig;

    public override void Use(GameObject tile)
    {
        shovelDig.Play();
        Plant plant = tile.GetComponentInChildren<Plant>();
        if (plant && plant.IsDead)
        {
            plant.Dig();
            shovelDig.Play();
        }
    }
}
