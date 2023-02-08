using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : Tool
{
    [SerializeField] private GameObject seedPrefab;
    public override void Use(GameObject tile)
    {
        FarmTile farmTile = tile.GetComponent<FarmTile>();
        if (farmTile.tilled == false || farmTile.occupiedBy != null)
        {
            return;
        }
        farmTile.occupiedBy = Instantiate(seedPrefab, tile.transform);
        
        
    }
}
