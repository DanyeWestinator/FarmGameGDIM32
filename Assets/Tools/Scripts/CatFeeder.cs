using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// logs nearby cats and attempts to feed them on use
public class CatFeeder : Tool
{   
    // cat food of choice - passed to cat
    public GameObject foodPrefab;
    
    // track nearby cat
    private CatBehavior cat = null;

    public override void Use(GameObject tile)
    {
       print("using cat feeder, cat is: " + cat);
       if (cat)
        {
            // todo params for player maybe (can tool get player)
            cat.FeedMe(null, foodPrefab);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {        
        // register nearby cat
        var comp = col.gameObject.GetComponent<CatBehavior>();
        if (comp)
        {
            cat = comp;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // deregister nearby cat 
        var comp = col.gameObject.GetComponent<CatBehavior>();
        if (comp)
        {
            cat = null;
        }
    }
}
