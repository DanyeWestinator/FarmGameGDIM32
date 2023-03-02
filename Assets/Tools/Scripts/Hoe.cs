using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : Tool
{

    public override void Use(GameObject tile)
    {
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
            if (p.Harvestable)
            {
                p.Harvest();
            }
        }
    }
}
