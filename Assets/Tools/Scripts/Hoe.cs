using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : Tool
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
        FarmTile farmTile = tile.GetComponent<FarmTile>();
        if (farmTile.tilled == false)
        {
            farmTile.SetTilled(true);
        }
    }
}
