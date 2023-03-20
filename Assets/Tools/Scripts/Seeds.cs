using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : Tool
{
    [SerializeField] private GameObject seedPrefab;
    public AudioSource plantingSeeds;
    public override void Use(GameObject tile)
    {
        plantingSeeds.Play();
        FarmTile farmTile = tile.GetComponent<FarmTile>();
        if (farmTile.tilled == false || farmTile.occupiedBy != null)
        {
            return;
        }
        farmTile.occupiedBy = Instantiate(seedPrefab, tile.transform);
        FarmSpawner.plantTiles[farmTile.occupiedBy.tileFromGO()] = farmTile.occupiedBy.GetComponent<Plant>();
        //FarmSpawner.plantTiles[]


    }
}
