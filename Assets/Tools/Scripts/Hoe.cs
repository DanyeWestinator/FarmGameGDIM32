using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Hoe : Tool
{
    public AudioSource hoeSound;
    public override void Use(GameObject tile)
    {
        hoeSound.Play();
        FarmTile farmTile = tile.GetComponent<FarmTile>();
        if (farmTile.tilled == false)
        {
            farmTile.SetTilled(true);
  
        }
        if (farmTile.occupiedBy == null)
            return;
        Plant p = farmTile.occupiedBy.GetComponent<Plant>();
        if (p)
        {
            //If we can harvest the plant, harvest it
            // (avoid harvesting catnip)
            if (p.Harvestable && !p.GetComponent<Catnip>())
            {
                p.Harvest();
            }
        }
    }
}

