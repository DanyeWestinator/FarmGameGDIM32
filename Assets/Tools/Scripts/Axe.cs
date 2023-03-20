using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Tool
{
    [SerializeField] private int treeChopScore = 1;
    public AudioSource axeChop;
 
    public override void Use(GameObject tile)
    {
        FarmTile farmTile = tile.GetComponent<FarmTile>();
        axeChop.Play();
        //Chops down trees
        if (farmTile.occupiedBy && farmTile.occupiedBy.name.Contains("tree"))
        {
            Destroy(farmTile.occupiedBy);
            farmTile.occupiedBy = null;
            PlayerController.player.AddScore(treeChopScore);
            axeChop.Play();
        }
    }
}
