using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Tool
{
    [SerializeField] private int treeChopScore = 1;
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
        FarmTile farmTile = tile.GetComponent<FarmTile>();
        if (farmTile.occupiedBy && farmTile.occupiedBy.name.Contains("tree"))
        {
            Destroy(farmTile.occupiedBy);
            farmTile.occupiedBy = null;
            PlayerController.player.AddScore(treeChopScore);
        }
    }
}
